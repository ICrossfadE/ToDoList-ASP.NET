using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ToDoList.Controllers
{
    public class ProfileController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

       [Authorize]
       [HttpPost]
       [Route("api/logout")]
       public  IActionResult Logout()
       {
           var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

           return Ok(new { message = "Успішний вихід з системи" });
       }

    }
}
