using AutoMapper;
using TodoApp.API.Entities.Users;
using TodoApp.API.Entities.Users.DTOs;

namespace TodoApp.API.Mapping
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<UpdateUserRequest, User>();
            CreateMap<User, ListUserResponse>();
            CreateMap<User, GetUserResponse>();
        }
    }
}
