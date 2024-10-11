using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using ToDoList.Models;
using ToDoList.Services;

namespace ToDoList.Controllers
{
    /* [ApiController]
     [Route("api/[controller]")]*/
    public class RegistrationController : Controller
    {
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;
        private readonly TodoDbContext _dbContext;

        public RegistrationController(IUserService userService, IConfiguration configuration, TodoDbContext dbContext)
        {
            _userService = userService;
            _configuration = configuration;
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        /*[Consumes("application/json")]*/
        public async Task<IActionResult> Register([FromForm] UserModel user)
        {
            // Перевірка на наявність користувача з такою ж поштою
            if (await _userService.UserExists(user.Email))
            {
                return BadRequest("Користувач з такою поштою вже існує.");
            }

            // Створення нового користувача
            var newUser = await _userService.CreateUserAsync(user);

            // Генерація JWT токена для нового користувача
            var token = GenerateJwtToken(newUser);

            return Ok(new { token });
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
                    new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
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


/*public class RegistrationController : Controller
{
    private readonly ILogger<RegistrationController> _logger;
    private readonly IConfiguration _configuration;
    private readonly TodoDbContext _db;
    *//*private readonly IAuthService _authService;
      private readonly IUserService _userService;*//*

    public RegistrationController
        (
        TodoDbContext context,
        ILogger<RegistrationController> logger,
        IConfiguration configuration
        *//*IAuthService authService,
        IUserService userService*//*
        )
    {
        _logger = logger;
        _configuration = configuration;
        _db = context;
        *//*_authService = authService;
        _userService = userService;*//*
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(UserModel user, string confirmPassword)
    {
        if (ModelState.IsValid)
        {
            if (user.Password == confirmPassword)
            {
                // Перевірка чи email вже існує
                var existingUser = await _db.Users.FirstOrDefaultAsync(u => u.Email == user.Email);
                if (existingUser != null)
                {
                    ModelState.AddModelError("Email", "Користувач з таким email вже існує.");
                    return View(user);
                }

                user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
                user.Roles = Request.Form["user-role"]; // Отримання обраної ролі

                _db.Users.Add(user);
                await _db.SaveChangesAsync();

                // Генерація JWT токена
                // var token = GenerateJwtToken(user);

                // Перенаправлення на сторінку з токеном
                // return RedirectToAction("Token", new { token = token });
            }
            else
            {
                ModelState.AddModelError("Password", "Паролі не співпадають.");
            }
        }
        return View(user);
    }

    public IActionResult Token(string token)
    {
        return View(model: token);
    }

    *//*private string GenerateJwtToken(UserModel user)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Roles)
        };

        var key = _authService.GetSymmetricSecurityKey();
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _authService.Issuer,
            audience: _authService.Audience,
            claims: claims,
            expires: DateTime.Now.AddMinutes(30),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }*//*
}*/