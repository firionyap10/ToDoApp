using AutoMapper;
using TodoApp.API.Data.ToDos;
using TodoApp.API.Entities.Commons.Operations;
using TodoApp.API.Entities.ToDos;
using TodoApp.API.Entities.ToDos.DTOs;

namespace TodoApp.API.Services.ToDos
{
    public class ToDoService : IToDoService
    {
        private readonly IToDoRespository _toDoRepository;
        private readonly IMapper _mapper;

        public ToDoService(IToDoRespository toDoRepository, IMapper mapper)
        {
            _toDoRepository = toDoRepository;
            _mapper = mapper;
        }

        public async Task<CreateToDoResponse> CreateAsync(CreateToDoRequest request)
        {
            var toDo = _mapper.Map<ToDo>(request);
            toDo.CreatedDate = DateTime.UtcNow;
            _toDoRepository.Add(toDo);
            await _toDoRepository.SaveAll();

            var result = await _toDoRepository.GetAsync(toDo.Id);

            var toDoResult = _mapper.Map<CreateToDoResponse>(result);

            return toDoResult;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var result = await _toDoRepository.GetAsync(id);
            _toDoRepository.Delete(result);
            return await _toDoRepository.SaveAll();
        }

        public async Task<GetToDoResponse> GetAsync(int id)
        {
            var toDo = await _toDoRepository.GetAsync(id);
            return _mapper.Map<GetToDoResponse>(toDo);
        }

        public async Task<PagedList<ListToDoResponse>> ListAsync(ListToDoRequest request)
        {
            var toDos = await _toDoRepository.ListAsync(request);
            
            return _mapper.Map<PagedList<ListToDoResponse>>(toDos);
        }

        public async Task<bool> UpdateAsync(UpdateToDoRequest request)
        {
            var result = _mapper.Map<ToDo>(request);
            
            return await _toDoRepository.UpdateAsync(result);
        }
    }
}
