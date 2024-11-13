using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.VisualBasic;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using user_service.model;
using user_service.Services;

namespace user_service.Producer
{
    public class UserProducer : BackgroundService
    {
        private readonly ConnectionFactory _factory;
        private readonly IServiceProvider _serviceProvider;

        private readonly IUserService _userService;

        public string userID;

        public UserProducer(IServiceProvider serviceProvider, string userId, IUserService userService)
        {

            userID = userId;
            _serviceProvider = serviceProvider;
            _factory = new ConnectionFactory();
            _factory.HostName = Environment.GetEnvironmentVariable("RABBITMQHOST");
            _userService = userService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var connection = _factory.CreateConnection();
            using var channel = connection.CreateModel();

            User user = await _userService.GetUser(userID);

            var body = Encoding.UTF8.GetBytes(user.ToString());

            channel.BasicPublish(exchange: "",
                                 routingKey: "user_detail",
                                 basicProperties: null,
                                 body: body);

            Console.WriteLine(" [x] Sent {0}", user);

            await Task.CompletedTask;
        }
    }
}
