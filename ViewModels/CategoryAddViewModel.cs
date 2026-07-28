using System.ComponentModel.DataAnnotations;
using PersonRegistrationMvc.Models;

namespace PersonRegistrationMvc.ViewModels
{
    public class CategoryAddViewModel
    {
        [Required(ErrorMessage = "Category name is required.")]
        [StringLength(50, ErrorMessage = "Category name cannot exceed 50 characters.")]
        public string Name { get; set; }

        public static Category ToEntity(CategoryAddViewModel viewModel)
        {
            return new Category { Name = viewModel.Name };
        }
    }
}