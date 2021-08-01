using Logic.ValidationAttributes;
using System.ComponentModel.DataAnnotations;

namespace Logic.Models
{
    public class RegisterRequest
    {
        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        [StringLength(250, MinimumLength = 2, ErrorMessage = "Количество символов должно быть от 2 до 250")]
        [Display(Name = "Фамилия Имя Отчество")]
        public string FIO { get; set; }

        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        [PhoneNumber(ErrorMessage = "Неверный формат телефона")]
        [Display(Name = "Телефон")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        [EmailAddress(ErrorMessage = "Неверный email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Количество символов должно быть от 3 до 20")]
        [UIHint("Password")]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        [UIHint("Password")]
        [Display(Name = "Подтверждение пароля")]
        public string PasswordConfirm { get; set; }
    }
}
