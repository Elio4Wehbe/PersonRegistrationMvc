namespace PersonRegistrationMvc.Models
{
    public class Author : AuditableEntity
    {
        public int AuthorId { get; set; }
        public string Name { get; set; }

        public ICollection<BookAuthor> BookAuthors { get; set; }
    }
}