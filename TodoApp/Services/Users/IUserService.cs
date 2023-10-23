using TodoApp.API.Entities.Commons.Operations;
using TodoApp.API.Entities.Users.DTOs;

namespace TodoApp.API.Services.Users
{
    public interface IUserService
    {
        Task<PagedList<ListUserResponse>> ListAsync(ListUserRequest request);

        Task<GetUserResponse> GetAsync(int id);

        Task<bool> UpdateAsync(UpdateUserRequest request);

        Task<UpdateUserRoleResponse> UpdateRoleAsync(UpdateUserRoleRequest request);
    }
}
