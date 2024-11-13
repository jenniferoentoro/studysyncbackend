// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Text;
// using System.Text.Json;
// using System.Threading.Tasks;
// using Docker.DotNet.Models;
// using user_service.Data;
// using user_service.Dtos;
// using user_service.model;
// using UserTest.Fixture;
// using static UserTest.InMemoryDb;
// using UserTest.Helper;

// namespace UserTest
// {
//     public class TestEnvironment : IClassFixture<WebApplicationFactoryFixture>
//     {
//         private WebApplicationFactoryFixture _factory;

//         public TestEnvironment(WebApplicationFactoryFixture factory)
//         {
//             _factory = factory;
//         }

//         [Fact]
//         public async Task GetUser()
//         {



//             var response = await _factory.Client.GetAsync(HttpHelper.Urls.getAllUsers);

//             var jsonContent = await response.Content.ReadAsStringAsync();

//             // Define custom converter for Grade enum
//            // var jsonOptions = new JsonSerializerOptions
//             //{
//               //  Converters = { new GradeConverter() }
//             //};

//             // Deserialize JSON response with custom converter
// //            var result = JsonSerializer.Deserialize<List<UserDto>>(jsonContent, jsonOptions);


//             response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
//   //          result.Count.Should().Be(_factory.InitialStudentCount);

//             // result.Should().HaveCount(1);

//             // result[0].Name.Should().Be("Test");
//             // result[0].Email.Should().Be("jenniferoentoro@gmail.com");

//         }
//     }
// }
