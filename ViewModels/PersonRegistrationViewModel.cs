using System.ComponentModel.DataAnnotations;
using PersonRegistrationMvc.Models;

namespace PersonRegistrationMvc.ViewModels
{
    public class PersonRegistrationViewModel
    {
        [Required(ErrorMessage = "FullNameRequired")]
        [StringLength(100, ErrorMessage = "FullNameMaxLength")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "AgeRequired")]
        [Range(1, 120, ErrorMessage = "AgeRange")]
        public int Age { get; set; }

        public Gender Gender { get; set; }

        [Required(ErrorMessage = "MajorRequiredMsg")]
        [StringLength(100, ErrorMessage = "MajorMaxLength")]
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