using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TodoApp.API.Controllers;
using TodoApp.API.Entities.Authentications;
using TodoApp.API.Services.Authentications;

namespace TodoApp.Tests.Controllers
{
    public class AuthControllerTests
    {
        private Mock<IAuthService> _mockAuthService;
        private AuthController _controller;

        [SetUp]
        public void Setup()
        {
            _mockAuthService = new Mock<IAuthService>();
            _controller = new AuthController(_mockAuthService.Object);
        }

        [Test]
        public async Task Register_WhenCalledAndHasErrors_ReturnsBadRequest()
        {
            var registerUserRequest = new RegisterUserRequest();
            _mockAuthService.Setup(service => service.RegisterAsync(registerUserRequest))
                .ReturnsAsync(new RegisterUserResponse { Errors = new List<IdentityError> {
                new IdentityError { }
                }  });

            var result = await _controller.Register(registerUserRequest);

            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
        }

        [Test]
        public async Task Register_WhenCalledAndHasNoErrors_ReturnsOk()
        {
            var registerUserRequest = new RegisterUserRequest();
            _mockAuthService.Setup(service => service.RegisterAsync(registerUserRequest))
                .ReturnsAsync(new RegisterUserResponse { Errors = null });

            var result = await _controller.Register(registerUserRequest);

            Assert.That(result, Is.InstanceOf<OkObjectResult>());
        }

        [Test]
        public async Task Login_WhenCalledWithInvalidCredentials_ReturnsUnauthorizedResult()
        {
            var loginUserRequest = new LoginUserRequest { };

            _mockAuthService.Setup(service => service.LoginAsync(loginUserRequest))
                .ReturnsAsync((LoginUserResponse)null);

            var result = await _controller.Login(loginUserRequest);

            Assert.That(result, Is.InstanceOf<UnauthorizedResult>());
        }

        [Test]
        public async Task Login_WhenCalledWithValidCredentials_ReturnsOkResult()
        {
            var loginUserRequest = new LoginUserRequest 
            {
                Username = "firionyap",
                Password = "password"
            };
            var expectedResponse = new LoginUserResponse 
            {
                Id = 14,
                Username = "firionyap",
                FirstName = "Firion",
                LastName = "Yap",
                DateOfBirth = DateTime.Parse("1992-10-23T00:00:00"),
                Token = "pbiIsIk1hbmnKWpeP2htoJIrOs3Q55tA2X0nTBXWueBmlAI42g"
            };

            _mockAuthService.Setup(service => service.LoginAsync(loginUserRequest))
                .ReturnsAsync(expectedResponse);

            var result = await _controller.Login(loginUserRequest);

            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.That(okResult.Value, Is.EqualTo(expectedResponse));
        }
    }
}
