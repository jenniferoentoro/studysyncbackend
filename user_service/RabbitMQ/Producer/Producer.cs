using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using RabbitMQ.Client;

namespace user_service.Producer
{
    public class MessageSender
    {
        private readonly string _key = "RHMZJ73Vfw3q6etU6+Y7zkAsQk6zpSdmFsVYmaArhmY=";
        private readonly string _iv = "U478xdTMwllL1IaTMXKAtw==";

        public void SendMessage(string message)
        {
            var encryptedMessage = EncryptStringToBytes_Aes(message, Convert.FromBase64String(_key), Convert.FromBase64String(_iv));
            var factory = new ConnectionFactory { HostName = Environment.GetEnvironmentVariable("RABBITMQHOST") };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.ExchangeDeclare(exchange: "logs", type: ExchangeType.Fanout);

            channel.BasicPublish(exchange: "logs",
                                 routingKey: string.Empty,
                                 basicProperties: null,
                                 body: encryptedMessage);
            Console.WriteLine($" [x] Sent encrypted message");
            Console.WriteLine($" [x] Sent {message}");
        }

        static byte[] EncryptStringToBytes_Aes(string plainText, byte[] Key, byte[] IV)
        {
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
    }
}
