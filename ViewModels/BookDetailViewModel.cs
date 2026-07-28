using PersonRegistrationMvc.Models;

namespace PersonRegistrationMvc.ViewModels
{
    public class BookDetailViewModel
    {
        public int BookId { get; set; }
        public string Title { get; set; }
        public int PublishedYear { get; set; }
        public int TotalCopies { get; set; }
        public int AvailableCopies { get; set; }
        public string CategoryName { get; set; }
        public List<string> AuthorNames { get; set; }

        public static BookDetailViewModel FromEntity(Book b) => new BookDetailViewModel
        {
            BookId = b.BookId,
            Title = b.Title,
            PublishedYear = b.PublishedYear,
            TotalCopies = b.TotalCopies,
            AvailableCopies = b.AvailableCopies,
            CategoryName = b.Category?.Name,
            AuthorNames = b.BookAuthors?.Select(ba => ba.Author?.Name).Where(n => n != null).ToList() ?? new List<string>()
        };
    }
}