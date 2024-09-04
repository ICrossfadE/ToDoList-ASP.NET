
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;

public class StatusController : Controller
{
    private readonly ILogger<StatusController> _logger;

    public StatusController(ILogger<StatusController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        var statusList = GetAllStatus();
        return View(statusList);
    }

    public AllStatusLitsModel GetAllStatus()
    {
        List<StatusModel> statusList = [];

        using SqliteConnection connection = new("Data Source=statuses.sqlite");
        using var tableCmd = connection.CreateCommand();
        connection.Open();
        tableCmd.CommandText = "SELECT * FROM todoStatus";

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

        return new AllStatusLitsModel
        {
            StatusList = statusList
        };
    }

    [HttpPost]
    public IActionResult Insert([FromBody] StatusModel statusItem)
    {
        using SqliteConnection connection = new("Data Source=statuses.sqlite");
        using var tableCmd = connection.CreateCommand();
        connection.Open();

        if(statusItem.Id != 0) 
        {
            // Update
            tableCmd.CommandText = "UPDATE todoStatus SET StatusName = @name WHERE Id = @id";
            tableCmd.Parameters.AddWithValue("@id", statusItem.Id);
            tableCmd.Parameters.AddWithValue("@name", statusItem.StatusName);
        }
        else
        {
            // Create
            tableCmd.CommandText = "INSERT INTO todoStatus (StatusName) VALUES (@name)";
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

    public StatusModel GetById(int id)
    {
        StatusModel statusItem = new();

        using SqliteConnection connection = new("Data Source=statuses.sqlite");
        using var tableCmd = connection.CreateCommand();
        connection.Open();
        tableCmd.CommandText = "SELECT * FROM todoStatus WHERE Id = @id";
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

    [HttpGet]
    public JsonResult UpdateStatus(int id)
    {
        var statusItem = GetById(id);
        return Json(statusItem);
    }

    [HttpDelete]
    public JsonResult Delete(int id)
    {
        using SqliteConnection connection = new("Data Source=statuses.sqlite");
        using var tableCmd = connection.CreateCommand();
        connection.Open();
        tableCmd.CommandText = "DELETE FROM todoStatus WHERE Id = @id";
        tableCmd.Parameters.AddWithValue("@id", id);
        tableCmd.ExecuteNonQuery();

        return Json(new {});
    }

}