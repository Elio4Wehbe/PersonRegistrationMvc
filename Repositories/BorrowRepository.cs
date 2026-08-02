using Microsoft.EntityFrameworkCore;
using PersonRegistrationMvc.Data;
using PersonRegistrationMvc.Models;

namespace PersonRegistrationMvc.Repositories
{
    public class BorrowRepository : IBorrowRepository
    {
        private readonly ApplicationDbContext _context;

        public BorrowRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Borrow> GetAll()
        {
            return _context.Borrows
                .Include(b => b.Person)
                .Include(b => b.Book)
                    .ThenInclude(b => b.Category)
                .ToList();
        }

        public void Add(Borrow borrow)
        {
            _context.Borrows.Add(borrow);
            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                throw new Exception("An error occurred while saving changes to the database. " + (ex.InnerException?.Message ?? ex.Message), ex);
            }
        }
        public Borrow GetById(int id)
        {
            return _context.Borrows.Find(id);
        }

        public void Update(Borrow borrow)
        {
            _context.Borrows.Update(borrow);
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
}