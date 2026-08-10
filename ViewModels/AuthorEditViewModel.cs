using System.ComponentModel.DataAnnotations;
using PersonRegistrationMvc.Models;

namespace PersonRegistrationMvc.ViewModels
{
    public class AuthorEditViewModel
    {
        public int AuthorId { get; set; }

        [Required(ErrorMessage = "AuthorNameRequired")]
        [StringLength(100, ErrorMessage = "AuthorNameMaxLength")]
        public string Name { get; set; }

        public static AuthorEditViewModel FromEntity(Author author)
        {
            return new AuthorEditViewModel { AuthorId = author.AuthorId, Name = author.Name };
        }

        public static void ApplyTo(AuthorEditViewModel viewModel, Author author)
        {
            author.Name = viewModel.Name;
        }
    }
}
