// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Net.Http.Json;
// using System.Threading.Tasks;
// using Microsoft.AspNetCore.Mvc.Testing;
// using Microsoft.AspNetCore.TestHost;
// using Microsoft.EntityFrameworkCore;
// using Microsoft.Extensions.DependencyInjection;
// using Microsoft.Extensions.DependencyInjection.Extensions;
// using user_service;
// using user_service.Data;
// using user_service.model;
// using user_service.Dtos;
// using FluentAssertions;
// using System.Text.Json;
// using System.Text.Json.Serialization;
// using Docker.DotNet.Models;
// using UserTest.Helper;

// namespace UserTest
// {
//     public class InMemoryDb
//     {
//         private WebApplicationFactory<Program> factory;

//         public InMemoryDb()
//         {
//             factory = new WebApplicationFactory<user_service.Program>().WithWebHostBuilder(builder =>
//             {
//                 builder.ConfigureTestServices(services =>
//                 {
//                     services.RemoveAll(typeof(DbContextOptions<AppDbContext>));
//                     services.AddDbContext<AppDbContext>(options =>
//                     {
//                         options.UseInMemoryDatabase("TestDb");
//                     });


//                 });

//             });


//         }

//         [Fact]
//         public async Task Test1()
//         {


//             using (var scope = factory.Services.CreateScope())
//             {
//                 var scopService = scope.ServiceProvider;
//                 var db = scopService.GetRequiredService<AppDbContext>();
//                 // db.Database.EnsureDeleted();
//                 db.Database.EnsureCreated();

//                 db.Users.Add(new User
//                 {
//                     Name = "Test",
//                     Email = "jenniferoentoro@gmail.com",
//                     Grade = user_service.Model.Grade.Elementary,
//                     School = "SDN 1",
//                     Role = user_service.Model.Role.Admin

//                 });
//                 db.SaveChanges();
//             }

//             var client = factory.CreateClient();
//             var response = await client.GetAsync(HttpHelper.Urls.getAllUsers);

//             var jsonContent = await response.Content.ReadAsStringAsync();

//             // Define custom converter for Grade enum
//             var jsonOptions = new JsonSerializerOptions
//             {
//                 Converters = { new GradeConverter() }
//             };

//             // Deserialize JSON response with custom converter
//             // var result = JsonSerializer.Deserialize<List<UserDto>>(jsonContent, jsonOptions);


//             response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

//             // result.Should().HaveCount(1);

//             // result[0].Name.Should().Be("Test");
//             // result[0].Email.Should().Be("jenniferoentoro@gmail.com");

//         }

//         public class GradeConverter : JsonConverter<user_service.Model.Grade>
//         {
//             public override user_service.Model.Grade Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
//             {
//                 string value = reader.GetString();
//                 if (Enum.TryParse<user_service.Model.Grade>(value, out user_service.Model.Grade grade))
//                 {
//                     return grade;
//                 }
//                 // Handle default value or throw exception if necessary
//                 return user_service.Model.Grade.Unknown;
//             }

//             public override void Write(Utf8JsonWriter writer, user_service.Model.Grade value, JsonSerializerOptions options)
//             {
//                 writer.WriteStringValue(value.ToString());
//             }
//         }
//     }
// }