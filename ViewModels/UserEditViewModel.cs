using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using PersonRegistrationMvc.Models;

namespace PersonRegistrationMvc.ViewModels
{
    public class UserEditViewModel
    {
        [Required]
        public string Id { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Please select a role.")]
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