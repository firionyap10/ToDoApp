using TodoApp.API.Entities.Commons.Operations;
using TodoApp.API.Entities.Commons.Repositories;
using TodoApp.API.Entities.Users;
using TodoApp.API.Entities.Users.DTOs;

namespace TodoApp.API.Data.Users
{
    public interface IUserRespository : IGenericRepository<User>
    {
        Task<bool> UpdateAsync(User user);

        Task<PagedList<User>> ListAsync(ListUserRequest request);

        Task<User> GetAsync(int id);
    }
}
