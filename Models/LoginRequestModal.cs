using System.ComponentModel.DataAnnotations;

namespace ToDoList.Models
{
    public class LoginRequestModal
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
