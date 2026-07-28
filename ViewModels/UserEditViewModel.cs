using System.ComponentModel.DataAnnotations;

namespace PersonRegistrationMvc.ViewModels
{
    public class UserEditViewModel
    {
        [Required]
        public string Id { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required]
        public string Role { get; set; }
    }
}
