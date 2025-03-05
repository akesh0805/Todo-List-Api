using Microsoft.EntityFrameworkCore;
using TodoList.Api.Data;
using TodoList.Api.Dtos;

namespace TodoList.Api.Services;

public class TodosService(TodoListDbContext context)
{
    public async Task<IEnumerable<Entites.Todo>> GetTodosAsync()
    {
        return await context.Todos.AsNoTracking().ToListAsync();
    }

    public async Task<Entites.Todo> GetTodoByIdAsync(Guid todoId)
    {
        return await context.Todos.Where(t => t.Id == todoId).FirstOrDefaultAsync() ?? throw new ArgumentException();
    }

    public async Task<Entites.Todo> CreateTodoAsync(Entites.Todo todo, CancellationToken cancellationToken = default)
    {
        var createdTodo = await context.Todos.AddAsync(todo, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        return createdTodo.Entity;
    }

    public async Task<Entites.Todo?> UpdateTodoAsync(Guid todoId, UpdateTodoDto updateTodoDto)
    {
        var existingTodo = await context.Todos.FirstOrDefaultAsync(t => t.Id == todoId);
        if (existingTodo is null)
        {
            return null; 
        }
        existingTodo.Title = updateTodoDto.Title;
        existingTodo.Description = updateTodoDto.Description;
        existingTodo.IsCompleted = updateTodoDto.IsCompleted;

        await context.SaveChangesAsync();

        return existingTodo;
    }

    public async Task<bool> DeleteTodoAsync(Guid todoId)
    {
        var existingTodo = await context.Todos.Where(t => t.Id == todoId).FirstOrDefaultAsync();
        if (existingTodo == null)
        {
            return false;
        }

        context.Todos.Remove(existingTodo);
        await context.SaveChangesAsync();
        return true;
    }
}