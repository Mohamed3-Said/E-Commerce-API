﻿using System.ComponentModel.DataAnnotations;

namespace Shared.Dtos
{
    public record RegisterDto
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        public string Email { get; init; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; init; }

        public string? PhoneNumber { get; init; }

        [Required(ErrorMessage = "DisplayName is required")]
        public string DisplayName { get; init; }

        [Required(ErrorMessage = "UserName is required")]
        public string UserName { get; init; }


    }
}
