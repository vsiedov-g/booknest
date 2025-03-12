using System;
using System.ComponentModel.DataAnnotations;

namespace booknest.Models.DTO
{
    public class AuthRequestData
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
