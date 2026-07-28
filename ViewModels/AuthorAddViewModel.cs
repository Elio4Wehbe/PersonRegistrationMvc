using System.ComponentModel.DataAnnotations;
using PersonRegistrationMvc.Models;

namespace PersonRegistrationMvc.ViewModels
{
    public class AuthorAddViewModel
    {
        [Required(ErrorMessage = "Author's name is required.")]
        [StringLength(100, ErrorMessage = "Author's name cannot exceed 100 characters.")]
        public string Name { get; set; }
        public static Author ToEntity(AuthorAddViewModel viewModel)
        {
            return new Author
            {
                Name = viewModel.Name
                
            };
        }
    }
}
