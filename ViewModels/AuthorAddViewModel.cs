using System.ComponentModel.DataAnnotations;
using PersonRegistrationMvc.Models;

namespace PersonRegistrationMvc.ViewModels
{
    public class AuthorAddViewModel
    {
        [Required(ErrorMessage = "AuthorNameRequired")]
        [StringLength(100, ErrorMessage = "AuthorNameMaxLength")]
        public string Name { get; set; }

        public static Author ToEntity(AuthorAddViewModel viewModel)
        {
            return new Author { Name = viewModel.Name };
        }
    }
}