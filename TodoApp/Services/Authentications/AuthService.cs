using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TodoApp.API.Entities.Authentications;
using TodoApp.API.Entities.Users;
using TodoApp.API.Services.Users;

namespace TodoApp.API.Services.Authentications
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _config;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;

        public AuthService(UserManager<User> userManager, SignInManager<User> signInManager,
                              IMapper mapper, IUserService userService, IConfiguration config)
        {
            _config = config;
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
            _userService = userService;
        }

        public async Task<LoginUserResponse> LoginAsync(LoginUserRequest loginUserRequest)
        {
            var response = new LoginUserResponse();
            var user = await _userManager.FindByNameAsync(loginUserRequest.Username);

            if(user == null)
                return null;

            var result = await _signInManager.CheckPasswordSignInAsync(user,
                loginUserRequest.Password, false);
            if (result.Succeeded)
            {
                var appUser = _userManager.Users
                    .FirstOrDefault(u => u.NormalizedUserName == loginUserRequest.Username.ToUpper());

                response = _mapper.Map<LoginUserResponse>(appUser);
                response.Token = await GenerateJwtToken(appUser);
                return response;
            }


            return null;
        }

        public async Task<RegisterUserResponse> RegisterAsync(RegisterUserRequest registerUserRequest)
        {
            var response = new RegisterUserResponse();
            var user = _mapper.Map<User>(registerUserRequest);
            user.CreatedDate = DateTime.UtcNow;

            var result = await _userManager.CreateAsync(user,
                registerUserRequest.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "Standard" );

                var userCreated = await _userService.GetAsync(user.Id);

                return _mapper.Map<RegisterUserResponse>(userCreated);
            }
            
            response.Errors = result.Errors;

            return response;
        }

        private async Task<string> GenerateJwtToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName)
            };

            var roles = await _userManager.GetRolesAsync(user);

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8
                .GetBytes(_config.GetSection("JwtSettings:Key").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
