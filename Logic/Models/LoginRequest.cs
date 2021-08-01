using Logic.ValidationAttributes;
using System.ComponentModel.DataAnnotations;

namespace Logic.Models
{
    public class LoginRequest
    {
        [Required(ErrorMessage = "Введите номер телефона")]
        [PhoneNumber(ErrorMessage = "Неверный формат телефона")]
        [Display(Name = "Телефон")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Введите пароль")]
        [UIHint("Password")]
        [Display(Name = "Пароль")]
        public string Password { get; set; }
    }
}
