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

        private static string ResolveName(string userId, Dictionary<string, string> userNames, string unknownUserLabel)
        {
            if (string.IsNullOrEmpty(userId)) return null;
            return userNames.TryGetValue(userId, out var name) ? name : unknownUserLabel;
        }

        public static AuditLogEntryViewModel FromPerson(Person person, Dictionary<string, string> userNames, string unknownUserLabel) => new AuditLogEntryViewModel
        {
            EntityType = "Person",
            EntityId = person.PersonId,
            EntityName = person.FullName,
            CreatedBy = ResolveName(person.CreatedBy, userNames, unknownUserLabel),
            CreatedAt = person.CreatedAt,
            UpdatedBy = ResolveName(person.UpdatedBy, userNames, unknownUserLabel),
            UpdatedAt = person.UpdatedAt
        };

        public static AuditLogEntryViewModel FromBook(Book book, Dictionary<string, string> userNames, string unknownUserLabel) => new AuditLogEntryViewModel
        {
            EntityType = "Book",
            EntityId = book.BookId,
            EntityName = book.Title,
            CreatedBy = ResolveName(book.CreatedBy, userNames, unknownUserLabel),
            CreatedAt = book.CreatedAt,
            UpdatedBy = ResolveName(book.UpdatedBy, userNames, unknownUserLabel),
            UpdatedAt = book.UpdatedAt
        };

        public static AuditLogEntryViewModel FromAuthor(Author author, Dictionary<string, string> userNames, string unknownUserLabel) => new AuditLogEntryViewModel
        {
            EntityType = "Author",
            EntityId = author.AuthorId,
            EntityName = author.Name,
            CreatedBy = ResolveName(author.CreatedBy, userNames, unknownUserLabel),
            CreatedAt = author.CreatedAt,
            UpdatedBy = ResolveName(author.UpdatedBy, userNames, unknownUserLabel),
            UpdatedAt = author.UpdatedAt
        };

        public static AuditLogEntryViewModel FromCategory(Category category, Dictionary<string, string> userNames, string unknownUserLabel) => new AuditLogEntryViewModel
        {
            EntityType = "Category",
            EntityId = category.CategoryId,
            EntityName = category.Name,
            CreatedBy = ResolveName(category.CreatedBy, userNames, unknownUserLabel),
            CreatedAt = category.CreatedAt,
            UpdatedBy = ResolveName(category.UpdatedBy, userNames, unknownUserLabel),
            UpdatedAt = category.UpdatedAt
        };

        public static AuditLogEntryViewModel FromBorrow(Borrow borrow, Dictionary<string, string> userNames, string unknownUserLabel) => new AuditLogEntryViewModel
        {
            EntityType = "Borrow",
            EntityId = borrow.BorrowId,
            EntityName = $"{borrow.Person?.FullName} — {borrow.Book?.Title}",
            CreatedBy = ResolveName(borrow.CreatedBy, userNames, unknownUserLabel),
            CreatedAt = borrow.CreatedAt,
            UpdatedBy = ResolveName(borrow.UpdatedBy, userNames, unknownUserLabel),
            UpdatedAt = borrow.UpdatedAt
        };

        public static AuditLogEntryViewModel FromUser(ApplicationUser user, Dictionary<string, string> userNames, string unknownUserLabel) => new AuditLogEntryViewModel
        {
            EntityType = "User",
            EntityId = 0,
            EntityName = $"{user.FullName} ({user.Email})",
            CreatedBy = ResolveName(user.CreatedBy, userNames, unknownUserLabel),
            CreatedAt = user.CreatedAt,
            UpdatedBy = ResolveName(user.UpdatedBy, userNames, unknownUserLabel),
            UpdatedAt = user.UpdatedAt
        };
    }
}