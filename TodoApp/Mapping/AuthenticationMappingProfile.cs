using AutoMapper;
using TodoApp.API.Entities.Authentications;
using TodoApp.API.Entities.Users;
using TodoApp.API.Entities.Users.DTOs;

namespace TodoApp.API.Mapping
{
    public class AuthenticationMappingProfile : Profile
    {
        public AuthenticationMappingProfile()
        {
            CreateMap<RegisterUserRequest, User>();
            CreateMap<User, RegisterUserResponse>();
            CreateMap<User, LoginUserResponse>();
            CreateMap<GetUserResponse, RegisterUserResponse>();
        }
    }
}
