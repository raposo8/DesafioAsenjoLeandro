using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ApiDesafio.ViewModels
{
    public class UserReturnViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
    }

    public class UserSignupViewModel
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required]
        [MinLength(6, ErrorMessage = "A senha deve ter pelo menos 6 caracteres.")]
        public string Password { get; set; } = string.Empty;
    }

    public class UserLoginViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
    }
}