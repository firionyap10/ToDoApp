using TodoApp.API.Entities.Authentications;
using TodoApp.API.Entities.Users;

namespace TodoApp.API.Services.Authentications
{
    public interface IAuthService
    {
        Task<LoginUserResponse> LoginAsync(LoginUserRequest loginUserRequest);

        Task<RegisterUserResponse> RegisterAsync(RegisterUserRequest registerUserRequest);
    }
}
