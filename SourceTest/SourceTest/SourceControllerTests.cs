using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using source_service.Controllers;
using source_service.Model;
using source_service.Dtos;
using System.Security.Claims;
using System.Collections.Generic;
using source_service.Service.Interface;
using source_service.RabbitMQ;
using source_service.Dtos.Source;
using source_service.Dtos.User;
using AutoMapper;
namespace SourceTest
{
    public class SourceControllerTests
    {
        private readonly Mock<ICacheService> _mockCacheService;
        private readonly Mock<ISourceService> _mockSourceService;
        private readonly Mock<ICategoryService> _mockCategoryService;
        private readonly Mock<IFileStorageService> _mockFileStorageService;
        private readonly Mock<RpcClient> _mockRpcClient;
        private readonly Mock<IUserService> _mockUserService;
        private readonly IFileService _fileService;
        private readonly IMapper _mapper;


        public SourceControllerTests()
        {
            _mockCacheService = new Mock<ICacheService>();
            _mockSourceService = new Mock<ISourceService>();
            _mockCategoryService = new Mock<ICategoryService>();
            _mockFileStorageService = new Mock<IFileStorageService>();
            _mockRpcClient = new Mock<RpcClient>();
            _mockUserService = new Mock<IUserService>();
        }

        [Fact]
        public async Task GetSource_Returns_Ok_With_Cached_Data()
        {

            // Arrange
            var sourceId = "667340b690517a82094bb8b7";
            UserDto user = new UserDto
            (
                "66730219a06674d48843902c",
                 "use8i2@example.com"
                );
            var expectedSourceDto = new SourceDto { Id = sourceId, User = user, privacy = "public" }; // Example expected DTO

            _mockCacheService.Setup(mock => mock.GetData<SourceDto>($"source{sourceId}")).Returns(expectedSourceDto);

            var controller = new SourceController(cacheService: _mockCacheService.Object, sourceService: _mockSourceService.Object, categoryService: _mockCategoryService.Object, fileStorageService: _mockFileStorageService.Object, mapper: _mapper, fileService: _fileService);

            // Act
            var result = await controller.GetSource(sourceId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var actualSourceDto = Assert.IsType<SourceDto>(okResult.Value);
            Assert.Equal(expectedSourceDto.privacy, actualSourceDto.privacy);
        }



    }
}
