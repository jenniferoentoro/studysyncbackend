// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Text;
// using System.Threading.Tasks;
// using Microsoft.AspNetCore.Mvc.Testing;
// using Microsoft.AspNetCore.TestHost;
// using Microsoft.EntityFrameworkCore;
// using Microsoft.Extensions.DependencyInjection;
// using Microsoft.Extensions.DependencyInjection.Extensions;
// using user_service;
// using user_service.Data;
// using user_service.model;

// namespace UserTest.Fixture
// {
//     public class WebApplicationFactoryFixture : IAsyncLifetime
//     {

//         private string connectionString = "Server=localhost;Port=5432;Username=postgres;Password=postgres;Database=user_service_test";
//         private WebApplicationFactory<Program> factory;

//         public HttpClient Client { get; private set; }

//         public int InitialStudentCount { get; set; } = 3;
//         public WebApplicationFactoryFixture()
//         {
//             factory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
//             {
//                 builder.ConfigureTestServices(services =>
//                 {
//                     services.RemoveAll(typeof(DbContextOptions<AppDbContext>));
//                     services.AddDbContext<AppDbContext>(options =>
//                     {
//                         options.UseNpgsql(connectionString);
//                     });


//                 });

//             });

//             Client = factory.CreateClient();
//         }


//         async Task IAsyncLifetime.InitializeAsync()
//         {
//             using (var scope = factory.Services.CreateScope())
//             {
//                 var scopService = scope.ServiceProvider;
//                 var db = scopService.GetRequiredService<AppDbContext>();
//                 // db.Database.EnsureDeleted();
//                 await db.Database.EnsureCreatedAsync();

//                 await db.Users.AddRangeAsync(DataFixture.GetUsers(InitialStudentCount));
//                 await db.SaveChangesAsync();

//             }
//         }

//         async Task IAsyncLifetime.DisposeAsync()
//         {
//             using (var scope = factory.Services.CreateScope())
//             {
//                 var scopService = scope.ServiceProvider;
//                 var db = scopService.GetRequiredService<AppDbContext>();
//                 // db.Database.EnsureDeleted();
//                 await db.Database.EnsureDeletedAsync();

//             }
//         }
//     }
// }
