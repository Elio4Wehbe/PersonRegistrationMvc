using PersonRegistrationMvc.Models;

namespace PersonRegistrationMvc.ViewModels
{
    public class AuditLogEntryViewModel
    {
        public string EntityType { get; set; }
        public int EntityId { get; set; }
        public string EntityName { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public static AuditLogEntryViewModel FromPerson(Person person) => new AuditLogEntryViewModel
        {
            EntityType = "Person",
            EntityId = person.PersonId,
            EntityName = person.FullName,
            CreatedBy = person.CreatedBy,
            CreatedAt = person.CreatedAt,
            UpdatedBy = person.UpdatedBy,
            UpdatedAt = person.UpdatedAt
        };

        public static AuditLogEntryViewModel FromBook(Book book) => new AuditLogEntryViewModel
        {
            EntityType = "Book",
            EntityId = book.BookId,
            EntityName = book.Title,
            CreatedBy = book.CreatedBy,
            CreatedAt = book.CreatedAt,
            UpdatedBy = book.UpdatedBy,
            UpdatedAt = book.UpdatedAt
        };

        public static AuditLogEntryViewModel FromAuthor(Author author) => new AuditLogEntryViewModel
        {
            EntityType = "Author",
            EntityId = author.AuthorId,
            EntityName = author.Name,
            CreatedBy = author.CreatedBy,
            CreatedAt = author.CreatedAt,
            UpdatedBy = author.UpdatedBy,
            UpdatedAt = author.UpdatedAt
        };

        public static AuditLogEntryViewModel FromCategory(Category category) => new AuditLogEntryViewModel
        {
            EntityType = "Category",
            EntityId = category.CategoryId,
            EntityName = category.Name,
            CreatedBy = category.CreatedBy,
            CreatedAt = category.CreatedAt,
            UpdatedBy = category.UpdatedBy,
            UpdatedAt = category.UpdatedAt
        };

        public static AuditLogEntryViewModel FromBorrow(Borrow borrow) => new AuditLogEntryViewModel
        {
            EntityType = "Borrow",
            EntityId = borrow.BorrowId,
            EntityName = $"{borrow.Person?.FullName} — {borrow.Book?.Title}",
            CreatedBy = borrow.CreatedBy,
            CreatedAt = borrow.CreatedAt,
            UpdatedBy = borrow.UpdatedBy,
            UpdatedAt = borrow.UpdatedAt
        };

        public static AuditLogEntryViewModel FromUser(ApplicationUser user) => new AuditLogEntryViewModel
        {
            EntityType = "User",
            EntityId = 0,
            EntityName = $"{user.FullName} ({user.Email})",
            CreatedBy = user.CreatedBy,
            CreatedAt = user.CreatedAt,
            UpdatedBy = user.UpdatedBy,
            UpdatedAt = user.UpdatedAt
        };
    }
}

