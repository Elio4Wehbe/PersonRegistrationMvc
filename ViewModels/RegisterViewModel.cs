using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace PersonRegistrationMvc.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        public string FullName { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required, DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Please select a role.")]
        public string RoleId { get; set; }

        [ValidateNever]
        public List<SelectListItem> RoleOptions { get; set; }
    }
}
