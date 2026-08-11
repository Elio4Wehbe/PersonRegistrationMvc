using System.ComponentModel.DataAnnotations;
using PersonRegistrationMvc.Models;

namespace PersonRegistrationMvc.ViewModels
{
    public class CategoryAddViewModel
    {
        [Required(ErrorMessage = "CategoryNameRequired")]
        [StringLength(50, ErrorMessage = "CategoryNameMaxLength")]
        public string Name { get; set; }

        public static Category ToEntity(CategoryAddViewModel viewModel)
        {
            return new Category { Name = viewModel.Name };
        }
    }
}