using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonRegistrationMvc.Repositories;
using PersonRegistrationMvc.ViewModels;

namespace PersonRegistrationMvc.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly IPersonRepository _personRepository;
        private readonly IBookRepository _bookRepository;
        private readonly IAuthorRepository _authorRepository;
        private readonly ICategoryRepository _categoryRepository;

        public DashboardController(
            IPersonRepository personRepository, IBookRepository bookRepository,
            IAuthorRepository authorRepository, ICategoryRepository categoryRepository)
        {
            _personRepository = personRepository;
            _bookRepository = bookRepository;
            _authorRepository = authorRepository;
            _categoryRepository = categoryRepository;
        }

        [HttpGet]
        public IActionResult Index(string searchTerm)
        {
            var viewModel = DashboardViewModel.Build(
                _personRepository.GetAll(), _bookRepository.GetAll(),
                _authorRepository.GetAll(), _categoryRepository.GetAll(), searchTerm);
            return View(viewModel);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Archived()
        {
            var archivedPersons = _personRepository.GetAllIncludingDeleted().Where(p => p.IsDeleted);
            var archivedBooks = _bookRepository.GetAllIncludingDeleted().Where(b => b.IsDeleted);

            var items = new List<ArchivedItemViewModel>();
            items.AddRange(archivedPersons.Select(ArchivedItemViewModel.FromPerson));
            items.AddRange(archivedBooks.Select(ArchivedItemViewModel.FromBook));

            var sorted = items.OrderByDescending(i => i.ArchivedDate).ToList();
            return View(sorted);
        }
    }
}