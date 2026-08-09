using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using PersonRegistrationMvc.Models;

namespace PersonRegistrationMvc.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "FullNameRequired")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "EmailRequired")]
        [EmailAddress(ErrorMessage = "EmailInvalid")]
        public string Email { get; set; }

        [Required(ErrorMessage = "PasswordRequired")]
        public string Password { get; set; }

        [Required(ErrorMessage = "RoleRequired")]
        public string RoleId { get; set; }

        public bool LinkExistingPerson { get; set; }

        public int? PersonId { get; set; }

        public int? Age { get; set; }
        public Gender? Gender { get; set; }
        public string? Major { get; set; }

        [ValidateNever]
        public List<SelectListItem> RoleOptions { get; set; }

        [ValidateNever]
        public List<SelectListItem> PersonOptions { get; set; } = new();
    }
}