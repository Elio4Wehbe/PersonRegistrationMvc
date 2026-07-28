using Microsoft.EntityFrameworkCore;
using PersonRegistrationMvc.Data;
using PersonRegistrationMvc.Models;

namespace PersonRegistrationMvc.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly ApplicationDbContext _context;

        public BookRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Book> GetAll()
        {
            return _context.Books
                .Include(b => b.Category)
                .Where(b => !b.IsDeleted)
                .ToList();
        }

        public Book? GetById(int id)
        {
            return _context.Books
                .Include(b => b.Category)
                .Include(b => b.BookAuthors)
                    .ThenInclude(ba => ba.Author)
                .FirstOrDefault(b => b.BookId == id);
        }

        public void Add(Book book)
        {
            _context.Books.Add(book);
            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                throw new Exception("An error occurred while saving changes to the database. " + (ex.InnerException?.Message ?? ex.Message), ex);
            }
        }

        public void Update(Book book)
        {
            _context.Books.Update(book);
            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                throw new Exception("An error occurred while saving changes to the database. " + (ex.InnerException?.Message ?? ex.Message), ex);
            }
        }

        public void Delete(int id)
        {
            var book = _context.Books.Find(id);
            if (book != null)
            {
                _context.Books.Remove(book);
                try
                {
                    _context.SaveChanges();
                }
                catch (DbUpdateException ex)
                {
                    throw new Exception("An error occurred while saving changes to the database. " + (ex.InnerException?.Message ?? ex.Message), ex);
                }
            }
        }
        public void SetAuthorsForBook(int bookId, List<int> authorIds)
        {
            var existingLinks = _context.BookAuthors.Where(ba => ba.BookId == bookId);
            _context.BookAuthors.RemoveRange(existingLinks);

            foreach (var authorId in authorIds)
            {
                _context.BookAuthors.Add(new BookAuthor { BookId = bookId, AuthorId = authorId });
            }

            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                throw new Exception("An error occurred while saving changes to the database. " + (ex.InnerException?.Message ?? ex.Message), ex);
            }
        }
        public List<Book> GetAllIncludingDeleted()
        {
            return _context.Books
                .Include(b => b.Category)
                .ToList();
        }

        public List<Book> GetByAuthorId(int authorId)
        {
            return _context.Books
                .Include(b => b.BookAuthors)
                .Where(b => !b.IsDeleted && b.BookAuthors.Any(ba => ba.AuthorId == authorId))
                .ToList();
        }
    }
}