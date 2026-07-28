using System.ComponentModel.DataAnnotations;
using PersonRegistrationMvc.Models;

namespace PersonRegistrationMvc.ViewModels
{
    public class CategoryEditViewModel
    {
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Category name is required.")]
        [StringLength(50, ErrorMessage = "Category name cannot exceed 50 characters.")]
        public string Name { get; set; }

        public static CategoryEditViewModel FromEntity(Category category)
        {
            return new CategoryEditViewModel { CategoryId = category.CategoryId, Name = category.Name };
        }

        public static void ApplyTo(CategoryEditViewModel viewModel, Category category)
        {
            category.Name = viewModel.Name;
        }
    }
}