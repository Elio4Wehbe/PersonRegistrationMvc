namespace PersonRegistrationMvc.Models
{
    public class Book : AuditableEntity
    {
        public int BookId { get; set; }
        public string Title { get; set; }
        public int PublishedYear { get; set; }
        public int TotalCopies { get; set; }
        public int AvailableCopies { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public ICollection<BookAuthor> BookAuthors { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? ArchivedDate { get; set; }
        public string? CoverImageUrl { get; set; }
    }
}