using System;
using System.ComponentModel.DataAnnotations;

namespace Data.Entities
{
    public class User
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public string FIO { get; set; }

        [Required]
        public string Phone { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public DateTime LastLogin { get; set; }
    }
}
