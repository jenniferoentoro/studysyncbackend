using Moq;
using user_service.model;
using user_service.Repository.Interfaces;
using user_service.Services;
namespace UserTest
{
    public class UserUnitTests
    {
        private Mock<IUserRepository> _userRepository;
        private UserService _userService;

        public UserUnitTests()
        {
            _userRepository = new Mock<IUserRepository>();
            _userService = new UserService(_userRepository.Object);
        }
        [Fact]
        public void GetAllUsers_Returns_The_Correct_Data()
        {

            // Arrange
            var users = new List<User>
            {
                new User { Id = "userid1", Name = "Jennifer"
                },

                new User { Id = "userid2", Name = "Oentoro"
                }
            };

            _userRepository.Setup(x => x.GetAllUsers()).ReturnsAsync(users);

            // Act
            var result = _userService.GetAllUsers().Result;

            // Assert
            _userRepository.Verify(x => x.GetAllUsers(), Times.Once);
            Assert.Equal(users, result);
        }

        [Fact]
        public void GetUser_Returns_The_Correct_Data()
        {
            // Arrange
            var user = new User
            {
                Id = "userid1",
                Name = "Jennifer"
            };

            _userRepository.Setup(x => x.GetUser(user.Id)).ReturnsAsync(user);

            // Act
            var result = _userService.GetUser(user.Id).Result;

            // Assert
            _userRepository.Verify(x => x.GetUser(user.Id), Times.Once);
            Assert.Equal(user, result);
        }

        [Fact]
        public void DeleteUser_Returns_The_Correct_Data()
        {
            // Arrange
            var user = new User
            {
                Id = "userid1",
                Name = "Jennifer"
            };

            _userRepository.Setup(x => x.DeleteUser(user.Id));

            // Act
            _userService.DeleteUser(user.Id);

            // Assert
            _userRepository.Verify(x => x.DeleteUser(user.Id), Times.Once);
        }
    }
}