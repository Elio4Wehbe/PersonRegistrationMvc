using System.ComponentModel.DataAnnotations;

namespace PersonRegistrationMvc.ViewModels
{
    public class ResetPasswordViewModel
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Token { get; set; }

        [Required(ErrorMessage = "NewPasswordRequired")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "ConfirmPasswordRequired")]
        [Compare("NewPassword", ErrorMessage = "PasswordsDoNotMatch")]
        public string ConfirmPassword { get; set; }
    }
}