using System.Diagnostics;
using Microsoft.Data.Sqlite;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Models;

namespace ToDoList.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        var todoList = GetAllTodos();
        return View(todoList);
    }

    internal TodoViewModel GetAllTodos()
{
    List<ToDo> todoList = [];

    using SqliteConnection connection = new("Data Source=db.sqlite");
    using var tableCmd = connection.CreateCommand();
    connection.Open();
    tableCmd.CommandText = "SELECT * FROM todo";

    using var reader = tableCmd.ExecuteReader();
    if (reader.HasRows)
    {
        while (reader.Read())
        {
            todoList.Add(
                new ToDo
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1)
                }
            );
        }
    }

    return new TodoViewModel
    {
        TodoList = todoList
    };
}


    public void Insert(ToDo todo)
    {
        using SqliteConnection connection = new("Data Source=db.sqlite");
        using var tableCmd = connection.CreateCommand();
        connection.Open();
        tableCmd.CommandText = $"INSERT INTO todo (name) VALUES ('{todo.Name}')";
        try
        {
            tableCmd.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}
