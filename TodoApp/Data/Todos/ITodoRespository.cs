using TodoApp.API.Entities.Commons.Operations;
using TodoApp.API.Entities.Commons.Repositories;
using TodoApp.API.Entities.ToDos;
using TodoApp.API.Entities.ToDos.DTOs;

namespace TodoApp.API.Data.ToDos
{
    public interface IToDoRespository : IGenericRepository<ToDo>
    {
        Task<bool> UpdateAsync(ToDo toDo);

        Task<PagedList<ToDo>> ListAsync(ListToDoRequest request);

        Task<ToDo> GetAsync(int id);
    }
}
