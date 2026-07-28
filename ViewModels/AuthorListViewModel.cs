using PersonRegistrationMvc.Models;

namespace PersonRegistrationMvc.ViewModels
{
    public class AuthorListViewModel
    {
        public int AuthorId { get; set; }
        public string Name { get; set; }
        
        public static AuthorListViewModel FromEntity(Author author)
        {
            return new AuthorListViewModel
            {
                AuthorId = author.AuthorId,
                Name = author.Name,
               
            };
        }
    }
}
