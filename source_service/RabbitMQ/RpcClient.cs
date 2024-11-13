using System;
using System.Collections.Concurrent;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Security.Cryptography;
namespace source_service.RabbitMQ
{
    public class RpcClient : IDisposable
    {
        private readonly IConnection connection;
        private readonly IModel channel;
        private readonly string queueName;
        private readonly EventingBasicConsumer consumer;
        private readonly BlockingCollection<string> respQueue = new BlockingCollection<string>();
        private readonly IBasicProperties props;
        private readonly ConcurrentDictionary<string, TaskCompletionSource<string>> callbackMapper = new();

        string key = "RHMZJ73Vfw3q6etU6+Y7zkAsQk6zpSdmFsVYmaArhmY=";
        string iv = "U478xdTMwllL1IaTMXKAtw==";


        private readonly string replyQueueName;
        public RpcClient(string queueName)
        {
            this.queueName = queueName;

            var factory = new ConnectionFactory()
            {
                HostName = Environment.GetEnvironmentVariable("RABBITMQHOST"),
                Port = int.Parse(Environment.GetEnvironmentVariable("RABBITMQPORT"))
            };

            connection = factory.CreateConnection();
            channel = connection.CreateModel();
            replyQueueName = channel.QueueDeclare().QueueName;

            consumer = new EventingBasicConsumer(channel);

            consumer.Received += (model, ea) =>
         {
             if (!callbackMapper.TryRemove(ea.BasicProperties.CorrelationId, out var tcs))
                 return;
             var body = ea.Body.ToArray();
             //  var response = Encoding.UTF8.GetString(body);
             //   byte[] encrypted = EncryptStringToBytes_Aes("original", Convert.FromBase64String(key), Convert.FromBase64String(iv));

             Console.WriteLine("Encrypted Get: " + Encoding.UTF8.GetString(body));
             string response = DecryptStringFromBytes_Aes(body, Convert.FromBase64String(key), Convert.FromBase64String(iv));

             tcs.TrySetResult(response);
         };

            channel.BasicConsume(consumer: consumer,
                                 queue: replyQueueName,
                                 autoAck: true);
        }

        public Task<string> Call(string message, CancellationToken cancellationToken = default)
        {

            IBasicProperties props = channel.CreateBasicProperties();
            var correlationId = Guid.NewGuid().ToString();
            props.CorrelationId = correlationId;
            props.ReplyTo = replyQueueName;
            var messageBytes = Encoding.UTF8.GetBytes(message);
            byte[] encrypted = EncryptStringToBytes_Aes(message, Convert.FromBase64String(key), Convert.FromBase64String(iv));

            Console.WriteLine("Encrypted Send: " + Encoding.UTF8.GetString(encrypted));
            // Console.WriteLine("Encrypted: " + Encoding.UTF8.GetString(encrypted));

            // Console.WriteLine("Original: " + DecryptStringFromBytes_Aes(encrypted, Convert.FromBase64String(key), Convert.FromBase64String(iv)));

            var tcs = new TaskCompletionSource<string>();
            callbackMapper.TryAdd(correlationId, tcs);

            channel.BasicPublish(exchange: string.Empty,
                                 routingKey: "users",
                                 basicProperties: props,
                                 body: encrypted);

            cancellationToken.Register(() => callbackMapper.TryRemove(correlationId, out _));
            return tcs.Task;
        }

        public void Dispose()
        {
            connection.Close();
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