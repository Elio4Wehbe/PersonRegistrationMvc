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

        
        private static string ResolveName(string userId, Dictionary<string, string> userNames)
        {
            if (string.IsNullOrEmpty(userId)) return null;
            return userNames.TryGetValue(userId, out var name) ? name : "Unknown user";
        }

        public static AuditLogEntryViewModel FromPerson(Person person, Dictionary<string, string> userNames) => new AuditLogEntryViewModel
        {
            EntityType = "Person",
            EntityId = person.PersonId,
            EntityName = person.FullName,
            CreatedBy = ResolveName(person.CreatedBy, userNames),
            CreatedAt = person.CreatedAt,
            UpdatedBy = ResolveName(person.UpdatedBy, userNames),
            UpdatedAt = person.UpdatedAt
        };

        public static AuditLogEntryViewModel FromBook(Book book, Dictionary<string, string> userNames) => new AuditLogEntryViewModel
        {
            EntityType = "Book",
            EntityId = book.BookId,
            EntityName = book.Title,
            CreatedBy = ResolveName(book.CreatedBy, userNames),
            CreatedAt = book.CreatedAt,
            UpdatedBy = ResolveName(book.UpdatedBy, userNames),
            UpdatedAt = book.UpdatedAt
        };

        public static AuditLogEntryViewModel FromAuthor(Author author, Dictionary<string, string> userNames) => new AuditLogEntryViewModel
        {
            EntityType = "Author",
            EntityId = author.AuthorId,
            EntityName = author.Name,
            CreatedBy = ResolveName(author.CreatedBy, userNames),
            CreatedAt = author.CreatedAt,
            UpdatedBy = ResolveName(author.UpdatedBy, userNames),
            UpdatedAt = author.UpdatedAt
        };

        public static AuditLogEntryViewModel FromCategory(Category category, Dictionary<string, string> userNames) => new AuditLogEntryViewModel
        {
            EntityType = "Category",
            EntityId = category.CategoryId,
            EntityName = category.Name,
            CreatedBy = ResolveName(category.CreatedBy, userNames),
            CreatedAt = category.CreatedAt,
            UpdatedBy = ResolveName(category.UpdatedBy, userNames),
            UpdatedAt = category.UpdatedAt
        };

        public static AuditLogEntryViewModel FromBorrow(Borrow borrow, Dictionary<string, string> userNames) => new AuditLogEntryViewModel
        {
            EntityType = "Borrow",
            EntityId = borrow.BorrowId,
            EntityName = $"{borrow.Person?.FullName} — {borrow.Book?.Title}",
            CreatedBy = ResolveName(borrow.CreatedBy, userNames),
            CreatedAt = borrow.CreatedAt,
            UpdatedBy = ResolveName(borrow.UpdatedBy, userNames),
            UpdatedAt = borrow.UpdatedAt
        };

        public static AuditLogEntryViewModel FromUser(ApplicationUser user, Dictionary<string, string> userNames) => new AuditLogEntryViewModel
        {
            EntityType = "User",
            EntityId = 0,
            EntityName = $"{user.FullName} ({user.Email})",
            CreatedBy = ResolveName(user.CreatedBy, userNames),
            CreatedAt = user.CreatedAt,
            UpdatedBy = ResolveName(user.UpdatedBy, userNames),
            UpdatedAt = user.UpdatedAt
        };
    }
}