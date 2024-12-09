using System;
using System.ComponentModel.DataAnnotations;

namespace OMSV1.Application.Dtos;

    public class RegisterDto
    {
        [Required]
        [MinLength(3)]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; }

        public List<string> Roles { get; set; } = new();
    }