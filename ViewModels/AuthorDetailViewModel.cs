using PersonRegistrationMvc.Models;

namespace PersonRegistrationMvc.ViewModels
{
    public class AuthorDetailViewModel
    {
        public int AuthorId { get; set; }
        public string Name { get; set; }
        public List<string> BookTitles { get; set; }

        public static AuthorDetailViewModel FromEntity(Author author, List<Book> books) => new AuthorDetailViewModel
        {
            AuthorId = author.AuthorId,
            Name = author.Name,
            BookTitles = books.Select(b => b.Title).ToList()
        };
    }
}
