using AutoMapper;
using Microsoft.AspNetCore.Identity;
using TodoApp.API.Data.Users;
using TodoApp.API.Entities.Commons.Operations;
using TodoApp.API.Entities.Users;
using TodoApp.API.Entities.Users.DTOs;

namespace TodoApp.API.Services.Users
{
    public class UserService : IUserService
    {
        private readonly IUserRespository _userRepository;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;

        public UserService(IUserRespository userRepository, IMapper mapper, UserManager<User> userManager)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<GetUserResponse> GetAsync(int id)
        {
            var user = await _userRepository.GetAsync(id);

            return _mapper.Map<GetUserResponse>(user);
        }

        public async Task<PagedList<ListUserResponse>> ListAsync(ListUserRequest request)
        {
            var users = await _userRepository.ListAsync(request);
            
            return _mapper.Map<PagedList<ListUserResponse>>(users);
        }

        public async Task<bool> UpdateAsync(UpdateUserRequest request)
        {
            var result = _mapper.Map<User>(request);
            
            return await _userRepository.UpdateAsync(result);
        }

        public async Task<UpdateUserRoleResponse> UpdateRoleAsync(UpdateUserRoleRequest request)
        {
            var response = new UpdateUserRoleResponse();
            var user = await _userManager.FindByIdAsync(request.Id.ToString());

            var userRoles = await _userManager.GetRolesAsync(user);

            var selectedRoles = request.RoleNames ?? new List<string>();

            var result = await _userManager.AddToRolesAsync(user,
                selectedRoles.Except(userRoles));

            if (!result.Succeeded)
                return null;

            result = await _userManager.RemoveFromRolesAsync(user,
                userRoles.Except(selectedRoles));

            if (!result.Succeeded)
                return null;

            response.RoleNames = (List<string>)await _userManager.GetRolesAsync(user);
            return response;
        }
    }
}
