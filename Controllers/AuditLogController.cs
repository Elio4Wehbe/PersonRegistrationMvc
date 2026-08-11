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
        private readonly Microsoft.Extensions.Localization.IStringLocalizer<SharedResource> _localizer;

        public AuditLogController(
            IPersonRepository personRepository,
            IBookRepository bookRepository,
            IAuthorRepository authorRepository,
            ICategoryRepository categoryRepository,
            IBorrowRepository borrowRepository,
            UserManager<ApplicationUser> userManager,
            Microsoft.Extensions.Localization.IStringLocalizer<SharedResource> localizer)
        {
            _personRepository = personRepository;
            _bookRepository = bookRepository;
            _authorRepository = authorRepository;
            _categoryRepository = categoryRepository;
            _borrowRepository = borrowRepository;
            _userManager = userManager;
            _localizer = localizer;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var allUsers = _userManager.Users.ToList();
            var userNames = allUsers.ToDictionary(u => u.Id, u => u.FullName);
            var unknownUserLabel = _localizer["UnknownUser"].Value;

            var entries = new List<AuditLogEntryViewModel>();

            entries.AddRange(_personRepository.GetAllIncludingDeleted().Select(p => AuditLogEntryViewModel.FromPerson(p, userNames, unknownUserLabel)));
            entries.AddRange(_bookRepository.GetAllIncludingDeleted().Select(b => AuditLogEntryViewModel.FromBook(b, userNames, unknownUserLabel)));
            entries.AddRange(_authorRepository.GetAll().Select(a => AuditLogEntryViewModel.FromAuthor(a, userNames, unknownUserLabel)));
            entries.AddRange(_categoryRepository.GetAll().Select(c => AuditLogEntryViewModel.FromCategory(c, userNames, unknownUserLabel)));
            entries.AddRange(_borrowRepository.GetAll().Select(b => AuditLogEntryViewModel.FromBorrow(b, userNames, unknownUserLabel)));
            entries.AddRange(allUsers.Select(u => AuditLogEntryViewModel.FromUser(u, userNames, unknownUserLabel)));

            var sorted = entries
                .OrderByDescending(e => e.UpdatedAt ?? e.CreatedAt)
                .ToList();

            return View(sorted);
        }
    }
}