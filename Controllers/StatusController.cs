
using Microsoft.AspNetCore.Mvc;

public class StatusController : Controller
{
    private readonly ILogger<StatusController> _logger;

    public StatusController(ILogger<StatusController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }
}