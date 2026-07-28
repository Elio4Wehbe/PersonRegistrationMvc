using PersonRegistrationMvc.Models;

namespace PersonRegistrationMvc.ViewModels
{
    public class BookListViewModel
    {
        public int BookId { get; set; }
        public string Title { get; set; }
        public int PublishedYear { get; set; }
        public string CategoryName { get; set; }

        public static BookListViewModel FromEntity(Book book)
        {
            return new BookListViewModel
            {
                BookId = book.BookId,
                Title = book.Title,
                CategoryName = book.Category?.Name,
                PublishedYear = book.PublishedYear
            };
        }
    }
}