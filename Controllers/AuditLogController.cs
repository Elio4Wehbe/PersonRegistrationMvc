using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PersonRegistrationMvc.Models;
using PersonRegistrationMvc.Repositories;
using PersonRegistrationMvc.ViewModels;

namespace PersonRegistrationMvc.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AuditLogController : Controller
    {
        private readonly IPersonRepository _personRepository;
        private readonly IBookRepository _bookRepository;
        private readonly IAuthorRepository _authorRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IBorrowRepository _borrowRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public AuditLogController(
            IPersonRepository personRepository,
            IBookRepository bookRepository,
            IAuthorRepository authorRepository,
            ICategoryRepository categoryRepository,
            IBorrowRepository borrowRepository,
            UserManager<ApplicationUser> userManager)
        {
            _personRepository = personRepository;
            _bookRepository = bookRepository;
            _authorRepository = authorRepository;
            _categoryRepository = categoryRepository;
            _borrowRepository = borrowRepository;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var entries = new List<AuditLogEntryViewModel>();

            entries.AddRange(_personRepository.GetAllIncludingDeleted().Select(AuditLogEntryViewModel.FromPerson));
            entries.AddRange(_bookRepository.GetAllIncludingDeleted().Select(AuditLogEntryViewModel.FromBook));
            entries.AddRange(_authorRepository.GetAll().Select(AuditLogEntryViewModel.FromAuthor));
            entries.AddRange(_categoryRepository.GetAll().Select(AuditLogEntryViewModel.FromCategory));
            entries.AddRange(_borrowRepository.GetAll().Select(AuditLogEntryViewModel.FromBorrow));
            entries.AddRange(_userManager.Users.ToList().Select(AuditLogEntryViewModel.FromUser));

            var sorted = entries
                .OrderByDescending(e => e.UpdatedAt ?? e.CreatedAt)
                .ToList();

            return View(sorted);
        }
    }
}

