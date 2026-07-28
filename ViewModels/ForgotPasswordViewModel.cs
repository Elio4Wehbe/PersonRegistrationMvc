using System.ComponentModel.DataAnnotations;

namespace PersonRegistrationMvc.ViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required, EmailAddress]
        public string Email { get; set; }
    }
}
