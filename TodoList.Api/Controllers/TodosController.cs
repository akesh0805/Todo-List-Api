using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoList.Api.Dtos;
using TodoList.Api.Services;

namespace TodoList.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TodosController(TodosService service) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetTodos()
    {
        var todos = await service.GetTodosAsync();
        return Ok(todos);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetTodoById(Guid id)
    {
        try
        {
            var todo = await service.GetTodoByIdAsync(id);
            return Ok(todo);
        }
        catch (ArgumentException)
        {
            return NotFound($"Todo with ID {id} not found.");
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateTodo(CreateTodo todo)
    {
        var subClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (subClaim is null)
        {
            return Unauthorized();
        }

        var userId = Guid.Parse(subClaim.Value);
        var toDo = new Entites.Todo
        {
            Title = todo.Title,
            Description = todo.Description,
            UserId = userId
        };

        var createdTodo = await service.CreateTodoAsync(toDo);
        return Ok(createdTodo);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTodo(Guid id, UpdateTodoDto updateTodoDto)
    {
        var updatedTodo = await service.UpdateTodoAsync(id, updateTodoDto);
        if (updatedTodo is null)
        {
            return NotFound($"Todo with ID {id} not found.");
        }
        return Ok(updatedTodo);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<bool>> DeleteTodo(Guid id)
    {
        var isDeleted = await service.DeleteTodoAsync(id);
        if (!isDeleted)
        {
            return NotFound($"Todo with ID {id} not found.");
        }

        return Ok(true);
    }
}