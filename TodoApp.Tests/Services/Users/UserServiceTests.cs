using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Moq;
using TodoApp.API.Data.Users;
using TodoApp.API.Entities.Commons.Operations;
using TodoApp.API.Entities.Enums;
using TodoApp.API.Entities.Users;
using TodoApp.API.Entities.Users.DTOs;
using TodoApp.API.Services.Users;

namespace TodoApp.Tests.Services.Users
{
    [TestFixture]
    public class UserServiceTests
    {
        private Mock<IUserRespository> _mockUserRepository;
        private Mock<IMapper> _mockMapper;
        private Mock<UserManager<User>> _mockUserManager;
        private UserService _service;

        [SetUp]
        public void Setup()
        {
            _mockUserRepository = new Mock<IUserRespository>();
            _mockMapper = new Mock<IMapper>();
            var store = new Mock<IUserStore<User>>();
            _mockUserManager = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);
            _service = new UserService(_mockUserRepository.Object, _mockMapper.Object, _mockUserManager.Object);
        }

        [Test]
        public async Task GetAsync_ValidId_ReturnsUser()
        {
            var userId = 1;
            var user = new User
            {
                Id = userId,
                UserName = "testuser",
                FirstName = "Firion",
                LastName = "Yap",
            };
            var response = new GetUserResponse
            {
                Id = userId,
                UserName = "testuser",
                FirstName = "Firion",
                LastName = "Yap",
            };

            _mockUserRepository.Setup(r => r.GetAsync(userId)).ReturnsAsync(user);
            _mockMapper.Setup(m => m.Map<GetUserResponse>(user)).Returns(response);

            var result = await _service.GetAsync(userId);

            Assert.AreEqual(response, result);
        }

        [Test]
        public async Task ListAsync_ValidRequest_ReturnsUsers()
        {
            var request = new ListUserRequest 
            {
                PageNumber = 1,
                PageSize = 10,
                OrderBy = BaseListOrderBy.Descending,
                OrderByName = "CreatedDate"
            };
            var pagedList = new PagedList<User>(new List<User>(), 1, 1, 1);
            var responseList = new PagedList<ListUserResponse>(new List<ListUserResponse>(), 1, 1, 1);

            _mockUserRepository.Setup(r => r.ListAsync(request)).ReturnsAsync(pagedList);
            _mockMapper.Setup(m => m.Map<PagedList<ListUserResponse>>(pagedList)).Returns(responseList);

            var result = await _service.ListAsync(request);

            Assert.AreEqual(responseList, result);
        }

        [Test]
        public async Task UpdateAsync_ValidRequest_UpdatesUser()
        {
            var request = new UpdateUserRequest
            {
                Id = 1,
                FirstName = "Firion",
                LastName = "Yap",
            };
            var mappedUser = new User
            {
                Id = 1,
                UserName = "testuser",
                FirstName = "Firion 2",
                LastName = "Yap",
            };

            _mockMapper.Setup(m => m.Map<User>(request)).Returns(mappedUser);
            _mockUserRepository.Setup(r => r.UpdateAsync(mappedUser)).ReturnsAsync(true);

            var result = await _service.UpdateAsync(request);

            _mockUserRepository.Verify(r => r.UpdateAsync(mappedUser), Times.Once);
            Assert.IsTrue(result);
        }

        [Test]
        public async Task UpdateRoleAsync_ValidRequest_UpdatesRolesSuccessfully()
        {
            var request = new UpdateUserRoleRequest { Id = 1, RoleNames = new List<string> { "Admin" } };
            var user = new User { Id = 1 };
            var roles = new List<string> { "Admin"};

            _mockUserManager.Setup(u => u.FindByIdAsync(request.Id.ToString())).ReturnsAsync(user);
            _mockUserManager.Setup(u => u.GetRolesAsync(user)).ReturnsAsync(roles);
            _mockUserManager.Setup(u => u.AddToRolesAsync(user, It.IsAny<IEnumerable<string>>())).ReturnsAsync(IdentityResult.Success);
            _mockUserManager.Setup(u => u.RemoveFromRolesAsync(user, It.IsAny<IEnumerable<string>>())).ReturnsAsync(IdentityResult.Success);

            var result = await _service.UpdateRoleAsync(request);

            _mockUserManager.Verify(u => u.AddToRolesAsync(user, It.IsAny<IEnumerable<string>>()), Times.Once);
            _mockUserManager.Verify(u => u.RemoveFromRolesAsync(user, It.IsAny<IEnumerable<string>>()), Times.Once);
            Assert.AreEqual(request.RoleNames, result.RoleNames);
        }
    }
}