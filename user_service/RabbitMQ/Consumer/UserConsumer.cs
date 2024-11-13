using System;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using user_service.model;
// using user_service.Producer;
using user_service.Services;
using System.Security.Cryptography;

namespace user_service.Consumer
{
    public class UserConsumer : IHostedService
    {
        private readonly ConnectionFactory _factory;
        private readonly IServiceProvider _serviceProvider;
        string key = "RHMZJ73Vfw3q6etU6+Y7zkAsQk6zpSdmFsVYmaArhmY=";
        string iv = "U478xdTMwllL1IaTMXKAtw==";

        private IConnection connection;
        private readonly IModel channel;

        public UserConsumer(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _factory = new ConnectionFactory
            {
                HostName = Environment.GetEnvironmentVariable("RABBITMQHOST"),
                Port = int.Parse(Environment.GetEnvironmentVariable("RABBITMQPORT")),
                DispatchConsumersAsync = true
            };
            CreateConnection();
            channel = connection.CreateModel();


        }

        private void CreateConnection()
        {
            int retries = 0;
            int maxRetries = 5;
            TimeSpan delay = TimeSpan.FromSeconds(10);
            while (retries < maxRetries)
            {
                try
                {
                    connection = _factory.CreateConnection();
                    break;
                }
                catch (Exception)
                {
                    retries++;
                    Task.Delay(delay).Wait();
                    Console.WriteLine($" [.] Retry {retries} of {maxRetries}");
                }
            }

            if (retries == maxRetries)
            {
                throw new Exception("Retry count exceeded");
            }
        }

        protected async Task ExecuteAsync()
        {
            Console.WriteLine("User consumer is starting...");

            var queue = "users";
            channel.QueueDeclare(
                    queue: queue,
                    durable: false,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);
            channel.BasicQos(0, 1, false);
            var consumer = new AsyncEventingBasicConsumer(channel);

            channel.BasicConsume(queue: queue, autoAck: false, consumer: consumer);
            consumer.Received += async (model, ea) =>
            {
                User response = null;
                var body = ea.Body;
                var props = ea.BasicProperties;
                var replyProps = channel.CreateBasicProperties();
                replyProps.CorrelationId = props.CorrelationId;
                try
                {
                    string message = DecryptStringFromBytes_Aes(body.ToArray(), Convert.FromBase64String(key), Convert.FromBase64String(iv));

                    // var message = Encoding.UTF8.GetString(body.ToArray());

                    Console.WriteLine(" [x] Received encrypted{0}", body.ToArray());
                    string n = message;

                    using var scope = _serviceProvider.CreateScope();
                    var userService = scope.ServiceProvider.GetRequiredService<IUserService>();
                    var userDetails = await userService.GetUser(n);
                    response = userDetails;
                }
                catch (Exception e)
                {
                    Console.WriteLine(" [.] " + e.Message);
                    response = new User();

                }
                finally
                {
                    // var responseBytes = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(response));

                    var responseBytes = EncryptStringToBytes_Aes(JsonSerializer.Serialize(response), Convert.FromBase64String(key), Convert.FromBase64String(iv));

                    Console.WriteLine(" [x] Sent encrypted {0}", responseBytes);
                    channel.BasicPublish(
                        exchange: "",
                        routingKey: props.ReplyTo,
                        basicProperties: replyProps,
                        body: responseBytes);
                    channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                }
            };

            // await Task.Delay(Timeout.Infinite, stoppingToken); // Run the service indefinitely
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            connection.Close();
            return Task.CompletedTask;

        }


        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await ExecuteAsync();
        }

        static byte[] EncryptStringToBytes_Aes(string plainText, byte[] Key, byte[] IV)
        {
            // Check arguments.
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");
            byte[] encrypted;


            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }

            return encrypted;
        }

        static string DecryptStringFromBytes_Aes(byte[] cipherText, byte[] Key, byte[] IV)
        {
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");

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
