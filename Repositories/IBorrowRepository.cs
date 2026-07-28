using PersonRegistrationMvc.Models;

namespace PersonRegistrationMvc.Repositories
{
    public interface IBorrowRepository
    {
        List<Borrow> GetAll();
        void Add(Borrow borrow);
        Borrow GetById(int id);
        void Update(Borrow borrow);
    }
}
