using TodoApp.API.Entities.Commons.Operations;
using TodoApp.API.Entities.ToDos.DTOs;

namespace TodoApp.API.Services.ToDos
{
    public interface IToDoService
    {
        Task<PagedList<ListToDoResponse>> ListAsync(ListToDoRequest request);

        Task<GetToDoResponse> GetAsync(int id);

        Task<CreateToDoResponse> CreateAsync(CreateToDoRequest request);

        Task<bool> UpdateAsync(UpdateToDoRequest request);

        Task<bool> DeleteAsync(int id);
    }
}
