using PersonRegistrationMvc.Models;

namespace PersonRegistrationMvc.ViewModels
{
    public class ArchivedItemViewModel
    {
        public int Id { get; set; }
        public string ItemType { get; set; }       
        public string DisplayName { get; set; }    
        public string Details { get; set; }         
        public DateTime? ArchivedDate { get; set; }

        public static ArchivedItemViewModel FromPerson(Person p) => new ArchivedItemViewModel
        {
            Id = p.PersonId,
            ItemType = "Person",
            DisplayName = p.FullName,
            Details = p.Major,
            ArchivedDate = p.ArchivedDate
        };

        public static ArchivedItemViewModel FromBook(Book b) => new ArchivedItemViewModel
        {
            Id = b.BookId,
            ItemType = "Book",
            DisplayName = b.Title,
            Details = b.Category?.Name,
            ArchivedDate = b.ArchivedDate
        };
    }
}