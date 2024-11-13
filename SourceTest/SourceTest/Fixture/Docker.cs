// using System;
// using System.Collections.Generic;
// using System.Threading.Tasks;
// using DotNet.Testcontainers.Builders;
// using DotNet.Testcontainers.Containers;
// using Microsoft.AspNetCore.Hosting;
// using Microsoft.AspNetCore.Mvc.Testing;
// using Microsoft.AspNetCore.TestHost;
// using Microsoft.Extensions.DependencyInjection;
// using Microsoft.Extensions.DependencyInjection.Extensions;
// using RabbitMQ.Client;
// using source_service;
// using user_service.Data;
// namespace SourceTest.Fixture
// {

//     public class Docker : WebApplicationFactory<Program>, IAsyncLifetime
//     {
//         private RabbitMqTestContainer rabbitMqContainer;

//         public Docker()
//         {
//             rabbitMqContainer = new RabbitMqTestContainer();
//         }

//         protected override void ConfigureWebHost(IWebHostBuilder builder)
//         {
//             var connectionString = rabbitMqContainer.GetConnectionString();
//             base.ConfigureWebHost(builder);
//             builder.ConfigureTestServices(services =>
//             {
//                 services.RemoveAll(typeof(ConnectionFactory));
//                 services.AddSingleton(_ => new ConnectionFactory { Uri = new Uri(connectionString) });
//             });
//         }

//         public async Task InitializeAsync()
//         {
//             await rabbitMqContainer.StartAsync();
//         }

//         public async Task DisposeAsync()
//         {
//             await rabbitMqContainer.DisposeAsync();
//         }
//     }

//     public class RabbitMqTestContainer : TestcontainerContainer
//     {
//         private const string RabbitMqImage = "rabbitmq:3-management";
//         private const int RabbitMqPort = 5672;

//         public RabbitMqTestContainer()
//             : base(new TestcontainersBuilder<TestcontainersContainer>()
//                 .WithImage(RabbitMqImage)
//                 .WithPortBinding(RabbitMqPort, RabbitMqPort)
//                 .WithEnvironment("RABBITMQ_DEFAULT_USER", "guest")
//                 .WithEnvironment("RABBITMQ_DEFAULT_PASS", "guest")
//                 .Build())
//         {
//         }

//         public string GetConnectionString()
//         {
//             var container = this.GetContainer();
//             var host = container.Hostname;
//             var port = container.GetMappedPublicPort(RabbitMqPort);
//             return $"amqp://guest:guest@{host}:{port}";
//         }
//     }
// }
