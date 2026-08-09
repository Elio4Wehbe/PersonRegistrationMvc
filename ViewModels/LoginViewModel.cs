using System.ComponentModel.DataAnnotations;

namespace PersonRegistrationMvc.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "EmailRequired")] //matches the key from the .resx files
        [EmailAddress(ErrorMessage = "EmailInvalid")]
        public string Email { get; set; }

        [Required(ErrorMessage = "PasswordRequired")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}