using ToDoList.Models;
using Microsoft.EntityFrameworkCore;

public class TodoDbContext : DbContext
{
    public DbSet<UserModel> Users { get; set; }
    public DbSet<ToDoModel> TodoItems { get; set; }
    public DbSet<StatusModel> Statuses { get; set; }
    public TodoDbContext(DbContextOptions<TodoDbContext> options)
        : base(options)
    {
       /* Database.EnsureCreated();*/
    }
}