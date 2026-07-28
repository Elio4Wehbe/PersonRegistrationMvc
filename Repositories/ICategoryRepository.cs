
using PersonRegistrationMvc.Models;

namespace PersonRegistrationMvc.Repositories
{
    public interface ICategoryRepository
    {
        List<Category> GetAll();
        Category? GetById(int id);
        void Add(Category category);
        void Update(Category category);
        void Delete(int id);
        bool HasBooks(int categoryId);
    }
}