using AutoMapper;
using TodoApp.API.Entities.Commons.Operations;
using TodoApp.API.Entities.ToDos;
using TodoApp.API.Entities.ToDos.DTOs;

namespace TodoApp.API.Mapping
{
    public class TodoMappingProfile : Profile
    {
        public TodoMappingProfile()
        {
            CreateMap<CreateToDoRequest, ToDo>();
            CreateMap<UpdateToDoRequest, ToDo>();
            CreateMap<ToDo, CreateToDoResponse>();
            CreateMap<ToDo, ListToDoResponse>();
            CreateMap<ToDo, GetToDoResponse>();
        }
    }
}
