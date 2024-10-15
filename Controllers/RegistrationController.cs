﻿using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
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

        public RegistrationController(
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
            return View();
        }

        [HttpPost]
        [Route("api/register")]
        public async Task<IActionResult> Register([FromBody] UserModel user)
        {
            // Перевірка на наявність користувача з такою ж поштою
            if (await _userService.UserExists(user.Email))
            {
                return BadRequest("Користувач з такою поштою вже існує.");
            }

            // Створення нового користувача
            var newUser = await _userService.CreateUserAsync(user);

            return Ok();
        }


       
    }
}
