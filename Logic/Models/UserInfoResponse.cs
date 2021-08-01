using System;
using System.ComponentModel.DataAnnotations;

namespace Logic.Models
{
    public class UserInfoResponse
    {
        [Display(Name = "Ф.И.О.")]
        public string FIO { get; set; }

        [Display(Name = "Телефон")]
        public string Phone { get; set; }
                
        public string Email { get; set; }

        [Display(Name = "Дата последней авторизации")]
        public DateTime LastLogin { get; set; }
    }
}
