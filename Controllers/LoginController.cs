using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ToDoList.Models;
using System.Threading.Tasks;
using ToDoList.Services;

namespace ToDoList.Controllers
{
    /*[ApiController]
    [Route("api/[controller]")]*/
    public class LoginController : Controller
    {
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;
        private readonly TodoDbContext _dbContext;// Сервіс для роботи з користувачами

        public LoginController(
            IUserService userService,
            IConfiguration configuration,
            TodoDbContext dbContext)
        {
            _userService = userService;
            _configuration = configuration;
            _dbContext = dbContext;
        }
        public IActionResult Index()
        {
            return View(); // Повертає представлення для логіну
        }

        [HttpPost]
        [Route("api/login")]
        /*[Consumes("application/json")]*/
        public async Task<IActionResult> Login([FromBody] LoginRequestModal request)
        {
            // Знайти користувача за логіном
            var user = await _userService.AuthenticateAsync(request.Email, request.Password);
            if (user == null)
            {
                return Unauthorized(); // Неправильний логін або пароль
            }

            // Генерація JWT токену
            var token = GenerateJwtToken(user);
            return Ok(new { Token = token });
        }

        // Метод для генерації JWT токена
        private string GenerateJwtToken(UserModel user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                     new Claim("UserId", user.UserId.ToString()),
                     new Claim(ClaimTypes.Email, user.Email),
                     new Claim(ClaimTypes.Role, user.Role)
                 }),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}


