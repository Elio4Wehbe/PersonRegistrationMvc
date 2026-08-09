using System.ComponentModel.DataAnnotations;

namespace PersonRegistrationMvc.ViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required(ErrorMessage = "EmailRequired")]
        [EmailAddress(ErrorMessage = "EmailInvalid")]
        public string Email { get; set; }
    }
}