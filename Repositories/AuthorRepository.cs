using Microsoft.EntityFrameworkCore;
using PersonRegistrationMvc.Data;
using PersonRegistrationMvc.Models;

namespace PersonRegistrationMvc.Repositories
{
    public class AuthorRepository : IAuthorRepository
    {
        private readonly ApplicationDbContext _context;

        public AuthorRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Author> GetAll() => _context.Authors.ToList();

        public Author GetById(int id) => _context.Authors.Find(id);

        public void Add(Author author)
        {
            _context.Authors.Add(author);
            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                throw new Exception("An error occurred while saving changes to the database. " + (ex.InnerException?.Message ?? ex.Message), ex);
            }
        }

        public void Update(Author author)
        {
            _context.Authors.Update(author);
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
            var author = _context.Authors.Find(id);
            if (author != null)
            {
                _context.Authors.Remove(author);
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
        public bool IsLinkedToBooks(int authorId)
        {
            return _context.BookAuthors.Any(ba => ba.AuthorId == authorId);
        }
    }
}