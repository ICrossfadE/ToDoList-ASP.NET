
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Data.Sqlite;
using ToDoList.Controllers;
using ToDoList.Models;

public class StatusController : Controller
{
    private readonly ILogger<StatusController> _logger;
    private readonly IConfiguration _configuration;

    public StatusController(ILogger<StatusController> logger, IConfiguration configuration)
    {
        _configuration = configuration;
        _logger = logger;
    }

    public IActionResult Index()
    {
        var statusList = GetAllStatus();
        return View(statusList);
    }

    public AllTodoListModel GetAllStatus()
    {
        List<StatusModel> statusList = [];

        string connectionString = _configuration.GetConnectionString("DefaultConnection");

        using (SqlConnection statusConnection = new SqlConnection(connectionString))
        {
            statusConnection.Open();

            using (var tableCmd = statusConnection.CreateCommand())
            {
                tableCmd.CommandText = "SELECT * FROM statuses";
                using var reader = tableCmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        statusList.Add(
                            new StatusModel
                            {
                                Id = reader.GetInt32(0),
                                StatusName = reader.GetString(1),
                            }
                        );
                    }
                }

                return new AllTodoListModel { StatusList = statusList };

            }


        }
    }

    public StatusModel GetById(int id)
    {
        StatusModel statusItem = new();

        string connectionString = _configuration.GetConnectionString("DefaultConnection");

        using SqlConnection connection = new(connectionString);
        using var tableCmd = connection.CreateCommand();
        connection.Open();
        tableCmd.CommandText = "SELECT * FROM statuses WHERE Id = @id";
        tableCmd.Parameters.AddWithValue("@id", id);

        using var reader = tableCmd.ExecuteReader();
        if (reader.HasRows)
        {
            while (reader.Read())
            {
                statusItem = new StatusModel
                {
                    Id = reader.GetInt32(0),
                    StatusName = reader.GetString(1),
                };
            }
        }

        return statusItem;
    }

    [HttpPost]
    public IActionResult Insert([FromBody] StatusModel statusItem)
    {
        string connectionString = _configuration.GetConnectionString("DefaultConnection");

        using SqlConnection connection = new(connectionString);
        using var tableCmd = connection.CreateCommand();
        connection.Open();

        if (statusItem.Id != 0)
        {
            // Update
            tableCmd.CommandText = "UPDATE statuses SET StatusName = @name WHERE Id = @id";
            tableCmd.Parameters.AddWithValue("@id", statusItem.Id);
            tableCmd.Parameters.AddWithValue("@name", statusItem.StatusName);
        }
        else
        {
            // Create
            tableCmd.CommandText = "INSERT INTO statuses (StatusName) VALUES (@name)";
            tableCmd.Parameters.AddWithValue("@name", statusItem.StatusName);
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



    [HttpGet]
    public JsonResult UpdateStatus(int id)
    {
        var statusItem = GetById(id);
        return Json(statusItem);
    }

    [HttpDelete]
    public JsonResult Delete(int id)
    {
        string connectionString = _configuration.GetConnectionString("DefaultConnection");

        using SqlConnection connection = new(connectionString);
        using var tableCmd = connection.CreateCommand();
        connection.Open();
        tableCmd.CommandText = "DELETE FROM statuses WHERE Id = @id";
        tableCmd.Parameters.AddWithValue("@id", id);
        tableCmd.ExecuteNonQuery();

        return Json(new { });
    }

}