using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Moq;
using TodoApp.API.Entities.Authentications;
using TodoApp.API.Entities.Users;
using TodoApp.API.Entities.Users.DTOs;
using TodoApp.API.Services.Authentications;
using TodoApp.API.Services.Users;

namespace TodoApp.Tests.Services.Authentications
{
    [TestFixture]
    public class AuthServiceTests
    {
        private Mock<UserManager<User>> _mockUserManager;
        private Mock<SignInManager<User>> _mockSignInManager;
        private Mock<IMapper> _mockMapper;
        private Mock<IConfiguration> _mockConfig;
        private Mock<IUserService> _mockUserService;

        private AuthService _authService;

        [SetUp]
        public void SetUp()
        {
            _mockUserManager = new Mock<UserManager<User>>(
                Mock.Of<IUserStore<User>>(), null, null, null, null, null, null, null, null);
            _mockSignInManager = new Mock<SignInManager<User>>(
                _mockUserManager.Object, Mock.Of<IHttpContextAccessor>(),
                Mock.Of<IUserClaimsPrincipalFactory<User>>(), null, null, null, null);
            _mockMapper = new Mock<IMapper>();
            _mockConfig = new Mock<IConfiguration>();
            var configuration = CreateTestConfiguration();
            _mockConfig.Setup(c => c.GetSection("JwtSettings:Key")).Returns(configuration.GetSection("JwtSettings:Key"));
            _mockUserService = new Mock<IUserService>();

            _authService = new AuthService(_mockUserManager.Object, _mockSignInManager.Object,
                _mockMapper.Object, _mockUserService.Object, _mockConfig.Object);
        }

        private IConfiguration CreateTestConfiguration()
        {
            var myConfiguration = new Dictionary<string, string>
            {
                {"JwtSettings:Key", "ibIeXTDzrpDFkVDkAWdLnid98disdsyscyhbdy2e72yuDDjjfDShdCCsassazDDFWS"}
            };

            return new ConfigurationBuilder()
                .AddInMemoryCollection(myConfiguration)
                .Build();
        }

        [Test]
        public async Task LoginAsync_UserNotFound_ReturnsNull()
        {
            _mockUserManager.Setup(um => um.FindByNameAsync(It.IsAny<string>()))
                .ReturnsAsync((User)null);

            var result = await _authService.LoginAsync(new LoginUserRequest { Username = "unknown" });

            Assert.IsNull(result);
        }

        [Test]
        public async Task LoginAsync_WrongPassword_ReturnsNull()
        {
            var user = new User { UserName = "testuser" };

            _mockUserManager.Setup(um => um.FindByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(user);
            _mockSignInManager.Setup(sm => sm.CheckPasswordSignInAsync(user, It.IsAny<string>(), false))
                .ReturnsAsync(SignInResult.Failed);

            var result = await _authService.LoginAsync(new LoginUserRequest { Username = "testuser", Password = "wrongpass" });

            Assert.IsNull(result);
        }


        [Test]
        public async Task LoginAsync_ValidCredentials_ReturnsUserWithToken()
        {
            var loginUserRequest = new LoginUserRequest 
            { 
                Username = "TestUser", 
                Password = "Password123" 
            };

            var user = new User 
            { 
                Id = 1, 
                UserName = loginUserRequest.Username, 
                NormalizedUserName = loginUserRequest.Username.ToUpper() 
            };

            _mockUserManager.Setup(um => um.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(user);
            _mockUserManager.Setup(um => um.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(new List<string> { "Admin", "Manager"});
            _mockSignInManager.Setup(sm => sm.CheckPasswordSignInAsync(user, loginUserRequest.Password, false)).ReturnsAsync(SignInResult.Success);
            _mockUserManager.Setup(um => um.Users).Returns(new List<User> { user }.AsQueryable());
            _mockMapper.Setup(m => m.Map<LoginUserResponse>(It.IsAny<User>())).Returns(new LoginUserResponse 
            { 
                Id = user.Id, 
                Username = user.UserName 
            });

            // Act
            var result = await _authService.LoginAsync(loginUserRequest);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(user.Id, result.Id);
            Assert.IsNotEmpty(result.Token);
        }

        [Test]
        public async Task RegisterAsync_UserAlreadyExists_ReturnsError()
        {

            var userRequest = new RegisterUserRequest
            {
                Username = "TestUser",
                Password = "Password123"
            };

            var user = new User
            {
                Id = 1,
                UserName = userRequest.Username,
                NormalizedUserName = userRequest.Username.ToUpper()
            };

            _mockUserManager.Setup(um => um.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "User already exists" }));

            _mockUserManager.Setup(um => um.AddToRoleAsync(It.IsAny<User>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "User already exists" }));

            _mockMapper.Setup(m => m.Map<User>(It.IsAny<RegisterUserRequest>())).Returns(new User
            {
                Id = user.Id,
                UserName = user.UserName
            });

            var result = await _authService.RegisterAsync(userRequest);

            Assert.IsNotNull(result.Errors);
            Assert.AreEqual("User already exists", result.Errors.FirstOrDefault()?.Description);
        }

        [Test]
        public async Task RegisterAsync_SuccessfulRegistration_ReturnsUser()
        {
            var userRequest = new RegisterUserRequest
            {
                Username = "testuser",
                Password = "password"
            };
            var user = new GetUserResponse
            {
                Id = 1,
                UserName = "testuser",
                FirstName = "Firion",
                LastName = "Yap",
            };
            var registeredUserResponse = new RegisterUserResponse 
            { 
                UserName = "testuser",
                FirstName = "testuser",
                LastName = "test",
            };

            _mockUserManager.Setup(um => um.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);
            _mockUserManager.Setup(um => um.AddToRoleAsync(It.IsAny<User>(), "Standard"))
                .ReturnsAsync(IdentityResult.Success);
            _mockUserService.Setup(us => us.GetAsync(It.IsAny<int>()))
                .ReturnsAsync(user);
            _mockMapper.Setup(m => m.Map<User>(It.IsAny<RegisterUserRequest>())).Returns(new User
            {
                Id = user.Id,
                UserName = "testuser",
                FirstName = "testuser",
                LastName = "test"
            });
            _mockMapper.Setup(m => m.Map<RegisterUserResponse>(It.IsAny<User>()))
                .Returns(registeredUserResponse);

            _mockMapper.Setup(m => m.Map<RegisterUserResponse>(It.IsAny<GetUserResponse>()))
    .           Returns(registeredUserResponse);

            var result = await _authService.RegisterAsync(userRequest);

            Assert.IsNotNull(result);
            Assert.AreEqual("testuser", result.UserName);
        }
    }
}