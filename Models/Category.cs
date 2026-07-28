namespace PersonRegistrationMvc.Models
{
    public class Category : AuditableEntity
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }

        public ICollection<Book> Books { get; set; }
    }
}