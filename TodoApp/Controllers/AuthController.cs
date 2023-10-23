using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoApp.API.Entities.Authentications;
using TodoApp.API.Services.Authentications;

namespace TodoApp.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterUserRequest registerUserRequest)
        {
            var result = await _authService.RegisterAsync(registerUserRequest);

            if(result.Errors != null && result.Errors.Count() > 0)
                return BadRequest(result.Errors);

            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginUserRequest loginUserRequest)
        {
            var result = await _authService.LoginAsync(loginUserRequest);

            if(result == null)
                return Unauthorized();

            return Ok(result);
        }
    }
}
