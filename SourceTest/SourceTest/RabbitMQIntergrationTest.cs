using RabbitMQ.Client;
using source_service.RabbitMQ.temp;
using Xunit;
using System.Text.Json;
using source_service.Dtos.User;
namespace SourceTest
{
    public class RabbitMQIntergrationTest
    {
        private const string RabbitMQHost = "localhost";
        private const int RabbitMQPort = 5672;
        private const string RabbitMQUsername = "guest";
        private const string RabbitMQPassword = "guest";

        [Fact]
        public void TestRabbitMQConnection()
        {
            // Arrange
            var factory = new ConnectionFactory
            {
                HostName = RabbitMQHost,
                Port = RabbitMQPort,
                UserName = RabbitMQUsername,
                Password = RabbitMQPassword
            };

            // Act
            IConnection connection = null;
            Assert.Null(connection); // Ensure connection is null initially
            Assert.Null(Record.Exception(() => connection = factory.CreateConnection())); // Attempt connection

            // Assert
            Assert.NotNull(connection); // Check if connection is not null
            Assert.True(connection.IsOpen); // Check if connection is open

            // // Cleanup
            // connection?.Close();
        }

        [Fact]
        public async Task TestRabbitMQCommunication()
        {
            // Arrange
            var userId = "66754a625222baf12469b3fe";
            var RpcClient = new RpcClient("users");

            Console.WriteLine("Sending request to RPC server...");

            // Act
            var response = await RpcClient.Call(userId, CancellationToken.None);
            Console.WriteLine("Response: " + response);

            // Assert
            Assert.NotNull(response);
            UserDto userDetails = JsonSerializer.Deserialize<UserDto>(response);

            var expectedResponseJson = new UserDto
            (
                "66754a625222baf12469b3fe",
                 "user@example.com"
                );

            Assert.Equal(expectedResponseJson.Id, userDetails.Id);
        }

    }
}

