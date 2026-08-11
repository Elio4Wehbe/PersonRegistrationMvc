using System.ComponentModel.DataAnnotations;
using PersonRegistrationMvc.Models;

namespace PersonRegistrationMvc.ViewModels
{
    public class CategoryEditViewModel
    {
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "CategoryNameRequired")]
        [StringLength(50, ErrorMessage = "CategoryNameMaxLength")]
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