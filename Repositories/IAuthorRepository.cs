using PersonRegistrationMvc.Models;

namespace PersonRegistrationMvc.Repositories
{
    public interface IAuthorRepository
    {
        List<Author> GetAll();
        Author GetById(int id);
        void Add(Author author);
        void Update(Author author);
        void Delete(int id);
        bool IsLinkedToBooks(int authorId);
    }
}