using System.ComponentModel.DataAnnotations;
using PersonRegistrationMvc.Models;

namespace PersonRegistrationMvc.ViewModels
{
    public class PersonEditViewModel
    {
        public int PersonId { get; set; }

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