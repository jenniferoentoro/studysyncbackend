using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using source_service.Service.Interface;

namespace source_service.Consumer
{
    public class SourceConsumer : IHostedService
    {
        private readonly string _key = "RHMZJ73Vfw3q6etU6+Y7zkAsQk6zpSdmFsVYmaArhmY=";
        private readonly string _iv = "U478xdTMwllL1IaTMXKAtw==";
        private IConnection _connection;
        private IModel _channel;
        private readonly IServiceProvider _serviceProvider;
        private Task _consumerTask;
        private CancellationTokenSource _cts;

        public SourceConsumer(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("Starting SourceConsumer...");
            _cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            _consumerTask = Task.Run(() => ConsumeWithRetries(5, TimeSpan.FromSeconds(10), _cts.Token), _cts.Token);
            return Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("Stopping SourceConsumer...");
            _cts.Cancel();
            await _consumerTask;
            _channel?.Close();
            _connection?.Close();
        }

        private async Task ConsumeWithRetries(int maxRetries, TimeSpan delay, CancellationToken cancellationToken)
        {
            int retries = 0;

            while (retries < maxRetries && !cancellationToken.IsCancellationRequested)
            {
                try
                {
                    Console.WriteLine($"Retry {retries + 1} of {maxRetries}");
                    await ConsumeAsync(cancellationToken);
                    break;
                }
                catch (Exception ex)
                {
                    retries++;
                    Console.WriteLine($"Error occurred: {ex.Message}");
                    if (retries < maxRetries)
                    {
                        await Task.Delay(delay, cancellationToken);
                    }
                }
            }

            if (retries == maxRetries)
            {
                throw new Exception("Retry count exceeded");
            }
        }

        private Task ConsumeAsync(CancellationToken cancellationToken)
        {
            var factory = new ConnectionFactory { HostName = Environment.GetEnvironmentVariable("RABBITMQHOST") };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.ExchangeDeclare(exchange: "logs", type: ExchangeType.Fanout);

            var queueName = _channel.QueueDeclare().QueueName;
            _channel.QueueBind(queue: queueName, exchange: "logs", routingKey: string.Empty);
            Console.WriteLine("Waiting for logs.");

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async (model, ea) =>
            {
                try
                {
                    byte[] body = ea.Body.ToArray();
                    var message = DecryptStringFromBytes_Aes(body, Convert.FromBase64String(_key), Convert.FromBase64String(_iv));
                    Console.WriteLine($"Received message: {message}");

                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var sourceService = scope.ServiceProvider.GetRequiredService<ISourceService>();
                        var sources = await sourceService.DeleteSourcesByUserId(message);

                        // Delete file storage for private sources
                        foreach (var source in sources)
                        {
                            if (source.privacy == "private")
                            {
                                var fileStorageService = scope.ServiceProvider.GetRequiredService<IFileStorageService>();
                                await fileStorageService.DeleteFileStorage(source.Id);
                            }

                            // Remove cache data related to each source
                            var cacheService = scope.ServiceProvider.GetRequiredService<ICacheService>();
                            cacheService.RemoveData($"source{source.Id}");
                        }

                        // Remove cached user sources and other related cache items
                        var cacheServiceAll = scope.ServiceProvider.GetRequiredService<ICacheService>();
                        cacheServiceAll.RemoveData($"sourcesuser{message}");
                        cacheServiceAll.RemoveBySubstring("False");
                        cacheServiceAll.RemoveBySubstring("True");

                        Console.WriteLine("[x] Done");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error processing message: {ex.Message}");
                }
            };

            _channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);

            // Keep the task alive to keep consuming messages
            return Task.Delay(Timeout.Infinite, cancellationToken).ContinueWith(_ => { }, TaskScheduler.Default);
        }

        static string DecryptStringFromBytes_Aes(byte[] cipherText, byte[] Key, byte[] IV)
        {
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException(nameof(cipherText));
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException(nameof(Key));
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException(nameof(IV));

            string plaintext = null;

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }

            return plaintext;
        }
    }
}
