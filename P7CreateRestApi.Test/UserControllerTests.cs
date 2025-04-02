using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using AutoMapper;
using Dot.Net.WebApi.Controllers;
using P7CreateRestApi.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace Dot.Net.WebApi.Tests
{
    [TestClass]
    public class UserControllerTests
    {
        private Mock<UserManager<IdentityUser>> _userManagerMock;
        private Mock<IMapper> _mapperMock;
        private Mock<ILogger<UserController>> _loggerMock;
        private UserController _controller;

        [TestInitialize]
        public void Setup()
        {
            _userManagerMock = new Mock<UserManager<IdentityUser>>(
                Mock.Of<IUserStore<IdentityUser>>(), null, null, null, null, null, null, null, null);
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILogger<UserController>>();
            _controller = new UserController(_userManagerMock.Object, _mapperMock.Object, _loggerMock.Object);
        }

        [TestMethod]
        public void GetAll_ShouldReturnAllUsers()
        {
            // Arrange
            var users = new List<IdentityUser> { new IdentityUser { Id = "1", UserName = "user1" } }.AsQueryable();
            _userManagerMock.Setup(x => x.Users).Returns(users);

            // Act
            var result = _controller.GetAll();

            // Assert
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
        }

        [TestMethod]
        public async Task GetById_UserExists_ShouldReturnUser()
        {
            // Arrange
            var user = new IdentityUser { Id = "1", UserName = "user1" };
            _userManagerMock.Setup(x => x.FindByIdAsync("1")).ReturnsAsync(user);

            // Act
            var result = await _controller.GetById("1");

            // Assert
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            Assert.AreEqual(user, okResult.Value);
        }

        [TestMethod]
        public async Task GetById_UserDoesNotExist_ShouldReturnNotFound()
        {
            // Arrange
            _userManagerMock.Setup(x => x.FindByIdAsync("1")).ReturnsAsync((IdentityUser)null);

            // Act
            var result = await _controller.GetById("1");

            // Assert
            Assert.IsInstanceOfType(result.Result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task Create_ValidUser_ShouldReturnCreated()
        {
            // Arrange
            var userDto = new UserDTO { Email = "test@example.com", Password = "Password123!" };
            var user = new IdentityUser { UserName = userDto.Email, Email = userDto.Email };
            _userManagerMock.Setup(x => x.CreateAsync(It.IsAny<IdentityUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await _controller.Create(userDto);

            // Assert
            var createdResult = result.Result as CreatedAtActionResult;
            Assert.IsNotNull(createdResult);
            Assert.AreEqual(201, createdResult.StatusCode);
        }

        [TestMethod]
        public async Task Update_UserExists_ShouldReturnNoContent()
        {
            // Arrange
            var userDto = new UserDTO { Id = 1, Email = "updated@example.com" };
            var user = new IdentityUser { Id = "1", UserName = "user1", Email = "old@example.com" };
            _userManagerMock.Setup(x => x.FindByIdAsync("1")).ReturnsAsync(user);
            _userManagerMock.Setup(x => x.UpdateAsync(user)).ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await _controller.Update("1", userDto);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NoContentResult));
        }

        [TestMethod]
        public async Task Delete_UserExists_ShouldReturnNoContent()
        {
            // Arrange
            var user = new IdentityUser { Id = "1", UserName = "user1" };
            _userManagerMock.Setup(x => x.FindByIdAsync("1")).ReturnsAsync(user);
            _userManagerMock.Setup(x => x.DeleteAsync(user)).ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await _controller.Delete("1");

            // Assert
            Assert.IsInstanceOfType(result, typeof(NoContentResult));
        }
    }
}
