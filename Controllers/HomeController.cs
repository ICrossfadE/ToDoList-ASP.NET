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

    internal ToDo GetById(int id)
    {
        ToDo todo = new();

        using SqliteConnection connection = new("Data Source=db.sqlite");
        using var tableCmd = connection.CreateCommand();
        connection.Open();
        tableCmd.CommandText = "SELECT * FROM todo WHERE Id = @id";
        tableCmd.Parameters.AddWithValue("@id", id);

        using var reader = tableCmd.ExecuteReader();
        if (reader.HasRows)
        {
            while (reader.Read())
            {
                todo = new ToDo
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1)
                };
            }
        }

        return todo;
    }


    [HttpGet]
    public JsonResult UpdateForm(int id)
    {
        var todo = GetById(id);
        return Json(todo);
    }


    public RedirectResult Insert(ToDo todo)
    
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

        return Redirect("http://localhost:5248");
    }

    [HttpPost]
    public JsonResult Delete(int id)
    {
        using SqliteConnection connection = new("Data Source=db.sqlite");
        using var tableCmd = connection.CreateCommand();
        connection.Open();
        tableCmd.CommandText = "DELETE FROM todo WHERE Id = @id";
        tableCmd.Parameters.AddWithValue("@id", id);
        tableCmd.ExecuteNonQuery();

        return Json(new {});
    }
    
    public RedirectResult Update(ToDo todo)
    {
        using SqliteConnection connection = new("Data Source=db.sqlite");
        using var tableCmd = connection.CreateCommand();
        connection.Open();
        tableCmd.CommandText = $"UPDATE todo SET Name = '{todo.Name}' WHERE Id = '{todo.Id}'";
        try
            {
                tableCmd.ExecuteNonQuery();
            }
        catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        return Redirect("http://localhost:5248");
    }
}
