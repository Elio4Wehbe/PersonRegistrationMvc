using System.ComponentModel.DataAnnotations;
using PersonRegistrationMvc.Models;

namespace PersonRegistrationMvc.ViewModels
{
    public class PersonRegistrationViewModel
    {
        [Required(ErrorMessage = "Full name is required.")]
        [StringLength(100, ErrorMessage = "Full name cannot exceed 100 characters.")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Age is required.")]
        [Range(1, 120, ErrorMessage = "Age must be between 1 and 120.")]
        public int Age { get; set; }

        public Gender Gender { get; set; }

        [Required(ErrorMessage = "Major is required.")]
        [StringLength(100, ErrorMessage = "Major cannot exceed 100 characters.")]
        public string Major { get; set; }

        public static Person ToEntity(PersonRegistrationViewModel viewModel)
        {
            return new Person
            {
                FullName = viewModel.FullName,
                Age = viewModel.Age,
                Gender = viewModel.Gender,
                Major = viewModel.Major
            };
        }
    }
}