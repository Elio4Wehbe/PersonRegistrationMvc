using System.ComponentModel.DataAnnotations;
using PersonRegistrationMvc.Models;

namespace PersonRegistrationMvc.ViewModels
{
    public class AuthorEditViewModel
    {
        public int AuthorId { get; set; }

        [Required(ErrorMessage = "Author's name is required.")]
        [StringLength(100, ErrorMessage = "Author's name cannot exceed 100 characters.")]
        public string Name { get; set; }
        public static AuthorEditViewModel FromEntity(Author author)
        {
            return new AuthorEditViewModel
            {
                AuthorId = author.AuthorId,
                Name = author.Name,

            };
        }
        public static void ApplyTo(AuthorEditViewModel viewModel, Author author)
        {
            author.Name = viewModel.Name;
        }
    }
}
