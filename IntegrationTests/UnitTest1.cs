
// using Microsoft.AspNetCore.Http;
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.VisualStudio.TestTools.UnitTesting;
// using source_service.Controllers;
// using source_service.Dtos.Source;
// using source_service.Model;
// using source_service.Service;
// using System.Net;
// using Moq;
// using System.Security.Claims;
// using System.Threading.Tasks;
// using user_service.model;

// namespace IntegrationTests;

// [TestClass]
// public class UnitTest1
// {

//     private readonly Mock<ICacheService> _mockCacheService = new Mock<ICacheService>();
//     private readonly Mock<ISourceService> _mockSourceService = new Mock<ISourceService>();
//     private readonly Mock<ICategoryService> _mockCategoryService = new Mock<ICategoryService>();
//     private readonly Mock<IFileStorageService> _mockFileStorageService = new Mock<IFileStorageService>();
//     private readonly Mock<IUserService> _mockUserService = new Mock<IUserService>();

//     [TestMethod]
//     public async Task GetSource_Returns_Ok_When_UserService_Available()
//     {
//         // Arrange
//         var controller = new SourceController(_mockCacheService.Object, _mockSourceService.Object,
//                                               _mockCategoryService.Object, _mockFileStorageService.Object,
//                                               _mockUserService.Object);

//         // Mock cache data
//         _mockCacheService.Setup(m => m.GetData<SourceDto>("source123")).Returns((SourceDto)null);

//         // Mock source service response
//         var mockSource = new Source { Id = "123", privacy = "public", UserId = "user123", CategoryId = "cat123" };
//         _mockSourceService.Setup(m => m.GetSource("123")).ReturnsAsync(mockSource);

//         // Mock category service response
//         var mockCategory = new Category { Id = "cat123", Name = "TestCategory" };
//         _mockCategoryService.Setup(m => m.GetCategoryById("cat123")).ReturnsAsync(mockCategory);

//         // Mock user service response
//         var mockUser = new UserDto { Id = "user123", Name = "TestUser" };
//         _mockUserService.Setup(m => m.GetUser("user123")).ReturnsAsync(mockUser);

//         // Mock HttpContext user
//         var claims = new Claim[] { new Claim("appid", "user123") };
//         var identity = new ClaimsIdentity(claims, "TestAuth");
//         var userPrincipal = new ClaimsPrincipal(identity);
//         controller.ControllerContext = new ControllerContext()
//         {
//             HttpContext = new DefaultHttpContext() { User = userPrincipal }
//         };

//         // Act
//         var result = await controller.GetSource("123");

//         // Assert
//         var okResult = result.Result as OkObjectResult;
//         Assert.IsNotNull(okResult);
//         Assert.AreEqual((int)HttpStatusCode.OK, okResult.StatusCode);
//         Assert.IsNotNull(okResult.Value);
//         var sourceDto = okResult.Value as SourceDto;
//         Assert.IsNotNull(sourceDto);
//         Assert.AreEqual("123", sourceDto.Id);
//         Assert.AreEqual("TestCategory", sourceDto.Category.Name);
//         Assert.AreEqual("TestUser", sourceDto.User.Name);
//     }
// }