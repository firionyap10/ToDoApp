using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoApp.API.Entities.Commons.Operations;
using TodoApp.API.Entities.ToDos;
using TodoApp.API.Entities.ToDos.DTOs;
using TodoApp.API.Services.ToDos;

namespace TodoApp.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ToDoController : ControllerBase
    {
        private readonly IToDoService _toDoService;

        public ToDoController(IToDoService toDoService)
        {
            _toDoService = toDoService;
        }

        [HttpGet("{id}")]
        [Authorize(Policy = "Standard")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _toDoService.GetAsync(id);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpGet]
        [Authorize(Policy = "Standard")]
        public async Task<IActionResult> List([FromQuery] ListToDoRequest request)
        {
            var result = await _toDoService.ListAsync(request);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpPost]
        [Authorize(Policy = "Standard")]
        public async Task<IActionResult> Create(CreateToDoRequest request)
        {
            var result = await _toDoService.CreateAsync(request);

            if (result == null)
                return BadRequest();
            return Ok(result);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "Standard")]
        public async Task<IActionResult> Update(int id, UpdateToDoRequest request)
        {
            request.Id = id;
            var result = await _toDoService.UpdateAsync(request);

            if (!result)
                return BadRequest();

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "Manager")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _toDoService.DeleteAsync(id);

            if(!result)
                return BadRequest();

            return Ok();
        }
    }
}