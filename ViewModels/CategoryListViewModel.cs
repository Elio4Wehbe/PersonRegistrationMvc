using PersonRegistrationMvc.Models;

namespace PersonRegistrationMvc.ViewModels
{
    public class CategoryListViewModel
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }

        public static CategoryListViewModel FromEntity(Category category)
        {
            return new CategoryListViewModel { CategoryId = category.CategoryId, Name = category.Name };
        }
    }
}