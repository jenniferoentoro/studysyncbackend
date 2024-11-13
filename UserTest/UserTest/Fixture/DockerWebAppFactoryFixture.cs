using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Containers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.OpenApi.Writers;
using Testcontainers.PostgreSql;
using user_service;
using user_service.Data;
namespace UserTest.Fixture
{
    public class DockerWebAppFactoryFixture : WebApplicationFactory<Program>, IAsyncLifetime
    {
        private PostgreSqlContainer db_container;

        public int InitialStudentCount { get; set; } = 3;
        public DockerWebAppFactoryFixture()
        {
            db_container = new PostgreSqlBuilder().Build();
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            var connectionString = db_container.GetConnectionString();
            base.ConfigureWebHost(builder);
            builder.ConfigureTestServices(services =>
            {
                services.RemoveAll(typeof(DbContextOptions<AppDbContext>));
                services.AddDbContext<AppDbContext>(options =>
                {
                    options.UseNpgsql(connectionString);
                });
            });
        }
        public async Task InitializeAsync()
        {
            await db_container.StartAsync();

            using (var scope = Services.CreateScope())
            {
                var scopService = scope.ServiceProvider;
                var db = scopService.GetRequiredService<AppDbContext>();
                // db.Database.EnsureDeleted();
                await db.Database.EnsureCreatedAsync();

                // await db.Users.AddRangeAsync(DataFixture.GetUsers(InitialStudentCount));
                // await db.SaveChangesAsync();
            }
        }

        public async Task DisposeAsync()
        {
            await db_container.DisposeAsync();
        }

    }
}