using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace ToDoList.Services
{
    public interface IAuthService
    {
        string Issuer { get; }
        string Audience { get; }
        SymmetricSecurityKey GetSymmetricSecurityKey();
    }

    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;
        public AuthService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string Issuer => _configuration["Jwt:Issuer"];
        public string Audience => _configuration["Jwt:Audience"];

        public SymmetricSecurityKey GetSymmetricSecurityKey() =>
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
    }
}
