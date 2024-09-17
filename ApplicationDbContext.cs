using ToDoList.Models;
using Microsoft.EntityFrameworkCore;

public class TodoDbContext : DbContext
{
    public TodoDbContext(DbContextOptions<TodoDbContext> options)
        : base(options)
    {
    }

    // DbSet для таблиць
    public DbSet<ToDoModel> TodoItems { get; set; }
}

public class StatusDbContext : DbContext
{
    public StatusDbContext(DbContextOptions<StatusDbContext> options)
        : base(options)
    {
    }

    // DbSet для таблиць
    public DbSet<StatusModel> Statuses { get; set; }
}