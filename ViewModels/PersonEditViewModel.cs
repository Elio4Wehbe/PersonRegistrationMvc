using System.ComponentModel.DataAnnotations;
using PersonRegistrationMvc.Models;

namespace PersonRegistrationMvc.ViewModels
{
    public class PersonEditViewModel
    {
        public int PersonId { get; set; }

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

        public static PersonEditViewModel FromEntity(Person person)
        {
            return new PersonEditViewModel
            {
                PersonId = person.PersonId,
                FullName = person.FullName,
                Age = person.Age,
                Gender = person.Gender,
                Major = person.Major
            };
        }

        public static void ApplyTo(PersonEditViewModel viewModel, Person person)
        {
            person.FullName = viewModel.FullName;
            person.Age = viewModel.Age;
            person.Gender = viewModel.Gender;
            person.Major = viewModel.Major;
        }
    }
}