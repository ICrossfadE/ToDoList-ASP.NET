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

    public AllTodoListModel GetAllTodos()
    {
        List<ToDoModel> todoList = [];

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
                    
                    new ToDoModel
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        Description = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                        Status = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                    }
                );
            }
        }

        return new AllTodoListModel
        {
            TodoList = todoList
        };
    }

    public ToDoModel GetById(int id)
    {
        ToDoModel todo = new();

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
                todo = new ToDoModel
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    Description = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                };
            }
        }

        return todo;
    }


    [HttpGet]
    public JsonResult UpdateTodo(int id)
    {
        var todo = GetById(id);
        return Json(todo);
    }


    [HttpPost]
    public IActionResult Insert([FromBody] ToDoModel todo)
    {
        using SqliteConnection connection = new("Data Source=db.sqlite");
        using var tableCmd = connection.CreateCommand();
        connection.Open();

        if(todo.Id != 0) 
        {
            // Update
            tableCmd.CommandText = "UPDATE todo SET Name = @name, Description = @description, Status = @status WHERE Id = @id";
            tableCmd.Parameters.AddWithValue("@id", todo.Id);
            tableCmd.Parameters.AddWithValue("@name", todo.Name);
            tableCmd.Parameters.AddWithValue("@description", todo.Description);
            tableCmd.Parameters.AddWithValue("@status", todo.Status ?? "");
        }
        else
        {
            // Create
            tableCmd.CommandText = "INSERT INTO todo (Name, Description, Status) VALUES (@name, @description, @status)";
            tableCmd.Parameters.AddWithValue("@name", todo.Name);
            tableCmd.Parameters.AddWithValue("@description", todo.Description);
            tableCmd.Parameters.AddWithValue("@status", todo.Status ?? "");
        }

        try
        {
            tableCmd.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return StatusCode(500, "Internal server error");
        }

        return Redirect("http://localhost:5248");
    }

    [HttpPost]
    public IActionResult SetStatus([FromBody] ToDoModel todo) 
    {

        using SqliteConnection connection = new("Data Source=db.sqlite");
        using var tableCmd = connection.CreateCommand();
        connection.Open();

        tableCmd.CommandText = "UPDATE todo SET Status = @status WHERE Id = @id";
        tableCmd.Parameters.AddWithValue("@id", todo.Id);
        tableCmd.Parameters.AddWithValue("@status", todo.Status);

        try
            {
                tableCmd.ExecuteNonQuery();
            }
        catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        return Ok();
    }


    [HttpDelete]
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
}
