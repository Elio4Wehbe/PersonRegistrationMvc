using Microsoft.EntityFrameworkCore;
using PersonRegistrationMvc.Data;
using PersonRegistrationMvc.Models;

namespace PersonRegistrationMvc.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _context;

        public CategoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Category> GetAll() => _context.Categories.ToList();

        public Category? GetById(int id) => _context.Categories.Find(id);

        public void Add(Category category)
        {
            _context.Categories.Add(category);
            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                throw new Exception("An error occurred while saving changes to the database. " + (ex.InnerException?.Message ?? ex.Message), ex);
            }
        }

        public void Update(Category category)
        {
            _context.Categories.Update(category);
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
            var category = _context.Categories.Find(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
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

        public bool HasBooks(int categoryId)
        {
            return _context.Books.Any(b => b.CategoryId == categoryId);
        }
    }
}
