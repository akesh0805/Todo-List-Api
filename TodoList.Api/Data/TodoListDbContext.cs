using Microsoft.EntityFrameworkCore;
using TodoList.Api.Entites;

namespace TodoList.Api.Data;

public class TodoListDbContext(DbContextOptions<TodoListDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<Todo> Todos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TodoListDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}