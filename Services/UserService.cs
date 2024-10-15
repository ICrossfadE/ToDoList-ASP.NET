using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Text;
using ToDoList.Models;

namespace ToDoList.Services
{
    public interface IUserService
    {
        Task<bool> UserExists(string email);
        Task<UserModel> CreateUserAsync(UserModel userModel);
        Task<UserModel> AuthenticateAsync(string email, string password);
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

        public async Task<UserModel> AuthenticateAsync(string email, string password)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);

            if (user == null)
            {
                return null;
            }

            var result = _passwordHasher.VerifyHashedPassword(user, user.Password, password);
            if (result == PasswordVerificationResult.Success)
            {
                return user;
            }

            return null;
        }
    }
}
