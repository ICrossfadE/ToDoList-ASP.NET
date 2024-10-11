using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ToDoList.Models;

namespace ToDoList.Services
{
    public interface IUserService
    {
        Task<bool> UserExists(string email);
        Task<UserModel> CreateUserAsync(UserModel userModel);
    }

    public class UserService : IUserService
    {
        private readonly TodoDbContext _dbContext;
        private readonly IPasswordHasher<UserModel> _passwordHasher;

        public UserService(TodoDbContext dbContext, IPasswordHasher<UserModel> passwordHasher)
        {
            _dbContext = dbContext;
            _passwordHasher = passwordHasher;
        }

        public async Task<bool> UserExists(string email)
        {
            return await _dbContext.Users.AnyAsync(u => u.Email == email);
        }

        public async Task<UserModel> CreateUserAsync(UserModel user)
        {
            // Хешування пароля перед збереженням
            user.Password = _passwordHasher.HashPassword(user, user.Password);

            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();

            return user;
        }
    }
}
