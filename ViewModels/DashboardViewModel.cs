using PersonRegistrationMvc.Models;

namespace PersonRegistrationMvc.ViewModels
{
    public class DashboardViewModel
    {
        public int PersonCount { get; set; }
        public int BookCount { get; set; }
        public int AuthorCount { get; set; }
        public int CategoryCount { get; set; }

        public string SearchTerm { get; set; }
        public List<PersonListViewModel> MatchingPersons { get; set; } = new();
        public List<BookListViewModel> MatchingBooks { get; set; } = new();
        public List<AuthorListViewModel> MatchingAuthors { get; set; } = new();
        public List<CategoryListViewModel> MatchingCategories { get; set; } = new();

        public static DashboardViewModel Build(List<Person> persons, List<Book> books,
            List<Author> authors, List<Category> categories, string searchTerm)
        {
            var vm = new DashboardViewModel
            {
                PersonCount = persons.Count,
                BookCount = books.Count,
                AuthorCount = authors.Count,
                CategoryCount = categories.Count,
                SearchTerm = searchTerm
            };

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                vm.MatchingPersons = persons.Where(p => p.FullName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                    .Select(PersonListViewModel.FromEntity).ToList();
                vm.MatchingBooks = books.Where(b => b.Title.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                    .Select(BookListViewModel.FromEntity).ToList();
                vm.MatchingAuthors = authors.Where(a => a.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                    .Select(AuthorListViewModel.FromEntity).ToList();
                vm.MatchingCategories = categories.Where(c => c.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                    .Select(CategoryListViewModel.FromEntity).ToList();
            }
            return vm;
        }
    }
}