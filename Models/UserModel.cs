using System.ComponentModel.DataAnnotations;

namespace ToDoList.Models
{
    public class UserModel
    {
        [Key]
        public int UserId { get; set; }

        public string Role { get; set; }

        [Required(ErrorMessage = "Пошта обов'язкова")]
        [StringLength(100, ErrorMessage = "Логін повинен бути між {2} та {1} символів.", MinimumLength = 3)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Пароль обов'язковий")]
        [StringLength(100, ErrorMessage = "Пароль повинен бути щонайменше {2} символів.", MinimumLength = 6)]
        public string Password { get; set; }
    }
}
