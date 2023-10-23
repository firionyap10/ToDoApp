using Microsoft.AspNetCore.Mvc;
using Moq;
using TodoApp.API.Controllers;
using TodoApp.API.Entities.Commons.Operations;
using TodoApp.API.Entities.Users.DTOs;
using TodoApp.API.Services.Users;

namespace TodoApp.Tests.Controllers
{
    [TestFixture]
    public class UserControllerTests
    {
        private Mock<IUserService> _mockUserService;
        private UserController _controller;

        [SetUp]
        public void SetUp()
        {
            _mockUserService = new Mock<IUserService>();
            _controller = new UserController(_mockUserService.Object);
        }

        [Test]
        public async Task Get_ShouldReturnUser_WhenUserExists()
        {
            // Arrange
            var userId = 1;
            var expectedUser = new GetUserResponse 
            {
                Id = 1,
                FirstName = "Firion",
                LastName = "Yap 2",
                DateOfBirth = DateTime.Parse("2023-10-20T01:23:02.025"),
            };

            _mockUserService.Setup(service => service.GetAsync(userId))
                .ReturnsAsync(expectedUser);

            // Act
            var result = await _controller.Get(userId);

            // Assert
            Assert.That(result, Is.TypeOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.That(okResult.Value, Is.EqualTo(expectedUser));
        }

        [Test]
        public async Task Get_ShouldReturnNotFound_WhenUserDoesNotExist()
        {
            // Arrange
            var userId = 1;
            _mockUserService.Setup(service => service.GetAsync(userId))
                .ReturnsAsync((GetUserResponse)null);

            // Act
            var result = await _controller.Get(userId);

            // Assert
            Assert.That(result, Is.TypeOf<NotFoundResult>());
        }

        [Test]
        public async Task List_ShouldReturnListOfUsers_WhenUsersExist()
        {
            // Arrange
            var listRequest = new ListUserRequest();
            var users = new PagedList<ListUserResponse>(new List<ListUserResponse>
            {
                new ListUserResponse
                {
                    Id = 1,
                    FirstName = "Firion",
                    LastName = "Yap 2",
                    DateOfBirth = DateTime.Parse("2023-10-20T01:23:02.025"),
                    CreatedDate = DateTime.Parse("2023-10-21T08:30:14.4777077"),
                },
                new ListUserResponse
                {
                    Id = 2,
                    FirstName = "Firion",
                    LastName = "Yap 2",
                    DateOfBirth = DateTime.Parse("2023-10-20T01:23:02.025"),
                    CreatedDate = DateTime.Parse("2023-10-21T08:30:14.4777077"),
                }
            }, 1, 1, 10);

            _mockUserService.Setup(service => service.ListAsync(listRequest))
                .ReturnsAsync(users);

            // Act
            var result = await _controller.List(listRequest);

            // Assert
            Assert.That(result, Is.TypeOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.That(okResult.Value, Is.EqualTo(users));
        }

        [Test]
        public async Task List_ShouldReturnNotFound_WhenNoUsersExist()
        {
            // Arrange
            var listRequest = new ListUserRequest();

            _mockUserService.Setup(service => service.ListAsync(listRequest))
                .ReturnsAsync((PagedList<ListUserResponse>)null);

            // Act
            var result = await _controller.List(listRequest);

            // Assert
            Assert.That(result, Is.TypeOf<NotFoundResult>());
        }

        [Test]
        public async Task Update_ShouldReturnNoContent_WhenUpdateSucceeds()
        {
            // Arrange
            var userId = 1;
            var updateUserRequest = new UpdateUserRequest { Id = userId };

            _mockUserService.Setup(service => service.UpdateAsync(updateUserRequest))
                .ReturnsAsync(true);

            // Act
            var result = await _controller.Update(userId, updateUserRequest);

            // Assert
            Assert.That(result, Is.TypeOf<NoContentResult>());
        }

        [Test]
        public async Task Update_ShouldReturnBadRequest_WhenUpdateFails()
        {
            // Arrange
            var userId = 1;
            var updateUserRequest = new UpdateUserRequest { Id = userId };

            _mockUserService.Setup(service => service.UpdateAsync(updateUserRequest))
                .ReturnsAsync(false);

            // Act
            var result = await _controller.Update(userId, updateUserRequest);

            // Assert
            Assert.That(result, Is.TypeOf<BadRequestResult>());
        }

        [Test]
        public async Task UpdateRoles_ShouldReturnOk_WhenRolesUpdateSucceeds()
        {
            // Arrange
            var userId = 1;
            var updateUserRoleRequest = new UpdateUserRoleRequest { Id = userId };

            _mockUserService.Setup(service => service.UpdateRoleAsync(updateUserRoleRequest))
                .ReturnsAsync(new UpdateUserRoleResponse 
                { 
                    Id = 1,
                    RoleNames = new List<string> {"Admin", "Manager", "User" }
                });

            // Act
            var result = await _controller.UpdateRoles(userId, updateUserRoleRequest);

            // Assert
            Assert.That(result, Is.TypeOf<OkResult>());
        }

        [Test]
        public async Task UpdateRoles_ShouldReturnBadRequest_WhenRolesUpdateFails()
        {
            // Arrange
            var userId = 1;
            var updateUserRoleRequest = new UpdateUserRoleRequest { Id = userId };

            _mockUserService.Setup(service => service.UpdateRoleAsync(updateUserRoleRequest))
                .ReturnsAsync((UpdateUserRoleResponse)null);

            // Act
            var result = await _controller.UpdateRoles(userId, updateUserRoleRequest);

            // Assert
            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());
        }
    }
}