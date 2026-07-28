using Microsoft.EntityFrameworkCore;
using PersonRegistrationMvc.Data;
using PersonRegistrationMvc.Models;

namespace PersonRegistrationMvc.Repositories
{
    public class PersonRepository : IPersonRepository
    {
        private readonly ApplicationDbContext _context;

        public PersonRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Person> GetAll()
        {
            return _context.Persons.Where(p => !p.IsDeleted).ToList();
        }

        public Person? GetById(int id)
        {
            return _context.Persons.Find(id);
        }

        public void Add(Person person)
        {
            _context.Persons.Add(person);
            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                throw new Exception("An error occurred while saving changes to the database. " + (ex.InnerException?.Message ?? ex.Message), ex);
            }
        }

        public void Update(Person person)
        {
            _context.Persons.Update(person);
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
            var person = _context.Persons.Find(id);
            if (person != null)
            {
                _context.Persons.Remove(person);
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
        public List<Person> GetAllIncludingDeleted()
        {
            return _context.Persons.ToList();
        }
    }
}