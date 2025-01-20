using System;
using System.ComponentModel.DataAnnotations;

namespace OMSV1.Application.Dtos;

    public class RegisterDto
    {
        [Required]
        [MinLength(3)]
        public required string UserName { get; set; }

        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [Required]
        [MinLength(6)]
        public required string Password { get; set; }

        public List<string> Roles { get; set; } = new();
    }