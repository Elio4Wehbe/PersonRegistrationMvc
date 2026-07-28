using PersonRegistrationMvc.Models;

namespace PersonRegistrationMvc.Repositories
{
    public interface IBookRepository
    {
        List<Book> GetAll();
        Book? GetById(int id);
        void Add(Book book);
        void Update(Book book);
        void Delete(int id);
        void SetAuthorsForBook(int bookId, List<int> authorIds);
        List<Book> GetAllIncludingDeleted();
    }
}