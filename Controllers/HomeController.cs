using System.Diagnostics;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Models;
using Microsoft.Extensions.Configuration;

namespace ToDoList.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;

        public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            var model = GetAllData();
            return View(model);
        }

        public AllTodoListModel GetAllData()
        {
            List<ToDoModel> todoList = [];
            List<StatusModel> statusList = [];


            string connectionString = _configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection todoConnection = new SqlConnection(connectionString))
            {
                todoConnection.Open();

                using (var tableCmd = todoConnection.CreateCommand())
                {
                    tableCmd.CommandText = "SELECT * FROM todo";
                    using var reader = tableCmd.ExecuteReader();
                    while (reader.Read())
                    {
                        todoList.Add(new ToDoModel
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Description = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                            StatusId = reader.IsDBNull(3) ? 0 : reader.GetInt32(3)
                        });
                    }
                }
            }

            using (SqlConnection statusConnection = new SqlConnection(connectionString))
            {
                statusConnection.Open();


                using (var tableCmd = statusConnection.CreateCommand())
                {
                    tableCmd.CommandText = "SELECT * FROM statuses";
                    using var reader = tableCmd.ExecuteReader();
                    while (reader.Read())
                    {
                        statusList.Add(new StatusModel
                        {
                            Id = reader.GetInt32(0),
                            StatusName = reader.GetString(1)
                        });
                    }
                }
            }

            return new AllTodoListModel
            {
                TodoList = todoList,
                StatusList = statusList
            };
        }

        public ToDoModel GetById(int id)
        {
            ToDoModel todo = new();

            string connectionString = _configuration.GetConnectionString("DefaultConnection");

            using SqlConnection connection = new(connectionString);
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
                        StatusId = reader.GetInt32(3)
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
            string connectionString = _configuration.GetConnectionString("DefaultConnection");

            using SqlConnection connection = new(connectionString);
            using var tableCmd = connection.CreateCommand();
            connection.Open();

            if (todo.Id != 0)
            {
                // Update
                tableCmd.CommandText = "UPDATE todo SET Name = @name, Description = @description, StatusId = @statusId WHERE Id = @id";
                tableCmd.Parameters.AddWithValue("@id", todo.Id);
                tableCmd.Parameters.AddWithValue("@name", todo.Name);
                tableCmd.Parameters.AddWithValue("@description", todo.Description);
                tableCmd.Parameters.AddWithValue("@statusId", todo.StatusId);
            }
            else
            {
                // Create
                tableCmd.CommandText = "INSERT INTO todo (Name, Description, StatusId) VALUES (@name, @description, @statusId)";
                tableCmd.Parameters.AddWithValue("@name", todo.Name);
                tableCmd.Parameters.AddWithValue("@description", todo.Description);
                tableCmd.Parameters.AddWithValue("@statusId", todo.StatusId);
            }

            try
            {
                tableCmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error executing database command.");
                return StatusCode(500, "Internal server error");
            }

            return Redirect("http://localhost:5248");
        }

        [HttpPost]
        public IActionResult SetStatusId([FromBody] ToDoModel todo)
        {
            if (todo == null || todo.Id == 0 || todo.StatusId == 0)
            {
                return BadRequest("Invalid data.");
            }

            string connectionString = _configuration.GetConnectionString("DefaultConnection");

            using SqlConnection connection = new(connectionString);
            using var tableCmd = connection.CreateCommand();
            connection.Open();

            tableCmd.CommandText = "UPDATE todo SET StatusId = @StatusId WHERE Id = @Id";
            tableCmd.Parameters.AddWithValue("@Id", todo.Id);
            tableCmd.Parameters.AddWithValue("@StatusId", todo.StatusId);

            try
            {
                tableCmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error executing database command.");
                return StatusCode(500, "Internal server error");
            }

            return Ok();
        }

        [HttpDelete]
        public JsonResult Delete(int id)
        {
            string connectionString = _configuration.GetConnectionString("DefaultConnection");

            using SqlConnection connection = new(connectionString);
            using var tableCmd = connection.CreateCommand();
            connection.Open();
            tableCmd.CommandText = "DELETE FROM todo WHERE Id = @id";
            tableCmd.Parameters.AddWithValue("@id", id);
            tableCmd.ExecuteNonQuery();

            return Json(new { });
        }
    }
}
