using PersonRegistrationMvc.Models;

namespace PersonRegistrationMvc.ViewModels
{
    public class PersonListViewModel
    {
        public int PersonId { get; set; }
        public string FullName { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }
        public string Major { get; set; }

        public static PersonListViewModel FromEntity(Person person)
        {
            return new PersonListViewModel
            {
                PersonId = person.PersonId,
                FullName = person.FullName,
                Age = person.Age,
                Gender = person.Gender.ToString(),
                Major = person.Major
            };
        }
    }
}