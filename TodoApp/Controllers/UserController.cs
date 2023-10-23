using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoApp.API.Entities.Commons.Operations;
using TodoApp.API.Entities.Users;
using TodoApp.API.Entities.Users.DTOs;
using TodoApp.API.Services.Users;

namespace TodoApp.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("{id}")]
        [Authorize(Policy = "Standard")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _userService.GetAsync(id);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpGet]
        [Authorize(Policy = "Manager")]
        public async Task<IActionResult> List([FromQuery] ListUserRequest request)
        {
            var result = await _userService.ListAsync(request);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "Standard")]
        public async Task<IActionResult> Update(int id, UpdateUserRequest request)
        {
            request.Id = id;
            var result = await _userService.UpdateAsync(request);

            if (!result)
                return BadRequest();

            return NoContent();
        }

        [HttpPut("roles/{id}")]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> UpdateRoles(int id, UpdateUserRoleRequest updateUserRoleRequest)
        {
            updateUserRoleRequest.Id = id;

            var result = await _userService.UpdateRoleAsync(updateUserRoleRequest);

            if (result == null)
                return BadRequest("Failed to add to roles.");

            return Ok();
        }
    }
}