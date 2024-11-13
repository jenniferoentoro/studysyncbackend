// using System;
// using System.Collections.Generic;
// using System.Threading.Tasks;
// using Docker.DotNet;
// using DotNet.Testcontainers.Builders;
// using DotNet.Testcontainers.Containers;
// using Microsoft.EntityFrameworkCore;
// using Microsoft.Extensions.Configuration;
// using user_service.Data;
// using user_service.model;
// using user_service.Repository;
// using Xunit;

// namespace UserTest
// {
//     public class UserIntegrationTests : IDisposable
//     {
//         private readonly string containerName = "user-integration-tests";
//         private readonly string db = "user-integration-tests_db";
//         private readonly string connectionString;
//         private readonly UserRepository _userRepository;
//         private readonly DockerClient dockerClient;
//         private readonly TestcontainersContainer container;
//         private readonly AppDbContext _dbContext;

//         public UserIntegrationTests()
//         {
//             try
//             {
//                 dockerClient = new DockerClientConfiguration(new Uri("unix:/var/run/docker.sock")).CreateClient();
//             }
//             catch (DockerApiException ex)
//             {
//                 throw new Exception("Failed to create Docker client. Make sure Docker is installed and running on your machine.", ex);
//             }

//             container = new TestcontainersBuilder<TestcontainersContainer>()
//                 .WithDockerEndpoint(new Uri("unix:/var/run/docker.sock"))
//                 .WithImage("postgres:latest")
//                 .WithName(containerName)
//                 .WithPortBinding("5432", "5434") // Updated to string arguments
//                 .WithEnvironment("POSTGRES_USER", "postgres")
//                 .WithEnvironment("POSTGRES_PASSWORD", "password")
//               //  .WithWaitStrategy(DotNet.Testcontainers.Wait.ForUnixContainer().UntilPortIsAvailable(5432)) // Explicit namespace
//                 .WithEnvironment("POSTGRES_DB", db)
//                 .Build();

//             container.StartAsync().GetAwaiter().GetResult();

//             var server = container.Hostname;
//             connectionString = $"Server={server};Port=5434;Database={db};Username=postgres;Password=password;SslMode=None;";

//             var options = new DbContextOptionsBuilder<AppDbContext>()
//                 .UseNpgsql(connectionString)
//              .Options;

//             _dbContext = new AppDbContext((IConfiguration)options);
//             _dbContext.Database.EnsureCreated();
//             _userRepository = new UserRepository(_dbContext);
//         }

//         [Fact]
//         public async Task GetAllUsers_Returns_The_Correct_Data()
//         {
//             // Arrange
//             var users = new List<User>
//             {
//                 new User { Id = 1, Name = "Jennifer" },
//                 new User { Id = 2, Name = "Oentoro" }
//             };

//             await _dbContext.Users.AddRangeAsync(users);
//             await _dbContext.SaveChangesAsync();

//             // Act
//             var result = await _userRepository.GetAllUsers();

//             // Assert
//           //  Assert.Equal(users.Count, result.Count);
//         }

//         public void Dispose()
//         {
//             container.StopAsync().GetAwaiter().GetResult();
//             // Use container.Remove() instead if RemoveAsync is not available
//           //  container.RemoveAsync().GetAwaiter().GetResult();
//             _dbContext.Dispose();
//         }
//     }
// }
