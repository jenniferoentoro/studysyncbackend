using Xunit;
using Moq;
using source_service.Helpers;
using source_service.Model;
using source_service.Repository.Interface;
using source_service.Service;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SourceTest
{
    public class SourceTest
    {
        private Mock<ISourceRepository> _sourceRepository;
        private SourceService _sourceService;

        public SourceTest()
        {
            _sourceRepository = new Mock<ISourceRepository>();
            _sourceService = new SourceService(_sourceRepository.Object);
        }

        [Fact]
        public async Task GetSources_Returns_The_Correct_Data()
        {
            // Arrange
            var sources = new List<Source>
            {
                new Source { Id = "sourceid1", Title = "Jennifer" },
                new Source { Id = "sourceid2", Title = "Oentoro" }
            };

            var query = new QueryObjectSource();

            _sourceRepository.Setup(x => x.GetSources(query)).ReturnsAsync(sources);

            // Act
            var result = await _sourceService.GetSources(query);

            // Assert
            Assert.NotNull(result);

            _sourceRepository.Verify(x => x.GetSources(query), Times.Once);


        }

        [Fact]
        public async Task GetSource_Returns_The_Correct_Data()
        {
            // Arrange
            var source = new Source { Id = "sourceid1", Title = "Jennifer" };

            _sourceRepository.Setup(x => x.GetSource("sourceid1")).ReturnsAsync(source);

            // Act
            var result = await _sourceService.GetSource("sourceid1");

            // Assert
            Assert.NotNull(result);

            _sourceRepository.Verify(x => x.GetSource("sourceid1"), Times.Once);
        }
    }
}
