using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PersonRegistrationMvc.Models;
using PersonRegistrationMvc.Repositories;
using PersonRegistrationMvc.ViewModels;

namespace PersonRegistrationMvc.Controllers
{
    [Authorize]
    public class BorrowController : Controller
    {
        private readonly IBorrowRepository _borrowRepository;
        private readonly IPersonRepository _personRepository;
        private readonly IBookRepository _bookRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly Microsoft.Extensions.Localization.IStringLocalizer<SharedResource> _localizer;

        public BorrowController(
            IBorrowRepository borrowRepository,
            IPersonRepository personRepository,
            IBookRepository bookRepository,
            ICategoryRepository categoryRepository,
            UserManager<ApplicationUser> userManager,
            Microsoft.Extensions.Localization.IStringLocalizer<SharedResource> localizer)
        {
            _borrowRepository = borrowRepository;
            _personRepository = personRepository;
            _bookRepository = bookRepository;
            _categoryRepository = categoryRepository;
            _userManager = userManager;
            _localizer = localizer;
        }

        private string GetCurrentUserId() => User.FindFirstValue(ClaimTypes.NameIdentifier);

        private List<SelectListItem> BuildPersonOptions() =>
            _personRepository.GetAll()
                .Select(p => new SelectListItem { Value = p.PersonId.ToString(), Text = p.FullName })
                .ToList();

        private List<SelectListItem> BuildBookOptions() =>
            _bookRepository.GetAll()
                .Select(b => new SelectListItem { Value = b.BookId.ToString(), Text = b.Title })
                .ToList();

        private List<SelectListItem> BuildCategoryOptions() =>
            _categoryRepository.GetAll()
                .Select(c => new SelectListItem { Value = c.CategoryId.ToString(), Text = c.Name })
                .ToList();

        [HttpGet]
        public async Task<IActionResult> Borrow(int? bookId)
        {
            bool isAdmin = User.IsInRole("Admin");

            var viewModel = new BorrowViewModel
            {
                IsAdmin = isAdmin,
                BookId = bookId ?? 0,
                BookOptions = BuildBookOptions(),
                CategoryOptions = BuildCategoryOptions()
            };

            if (isAdmin)
            {
                viewModel.PersonOptions = BuildPersonOptions();
            }
            else
            {
                var currentUser = await _userManager.FindByIdAsync(GetCurrentUserId());
                if (currentUser?.PersonId == null)
                {
                    TempData["ErrorMessage"] = _localizer["NotLinkedToMemberMessage"].Value;
                    return RedirectToAction("Index");
                }
                viewModel.PersonId = currentUser.PersonId.Value;
            }

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Borrow(BorrowViewModel viewModel)
        {
            bool isAdmin = User.IsInRole("Admin");

            if (!isAdmin)
            {
                var currentUser = await _userManager.FindByIdAsync(GetCurrentUserId());
                if (currentUser?.PersonId == null)
                {
                    TempData["ErrorMessage"] = _localizer["NotLinkedToMemberMessage"].Value;
                    return RedirectToAction("Index");
                }
                // Non-admins never send PersonId from the form (it's hidden), so overwrite
                // whatever bound and clear the Range-validation error that value would trip.
                viewModel.PersonId = currentUser.PersonId.Value;
                ModelState.Remove(nameof(viewModel.PersonId));
            }

            viewModel.BorrowDate = DateTime.UtcNow;
            viewModel.DueDate = viewModel.BorrowDate.AddDays(14);

            var book = _bookRepository.GetById(viewModel.BookId);

            if (book == null)
            {
                ModelState.AddModelError("", _localizer["BookNotFoundMessage"]);
            }
            else if (book.AvailableCopies < 1)
            {
                ModelState.AddModelError("", _localizer["BookOutOfStockMessage"]);
            }

            if (ModelState.IsValid)
            {
                int activeBorrowCount = _borrowRepository.GetAll()
                    .Count(b => b.PersonId == viewModel.PersonId && b.ReturnDate == null);

                if (activeBorrowCount >= 3)
                {
                    ModelState.AddModelError("", _localizer["MaxActiveBorrowsMessage"]);
                }
            }

            if (!ModelState.IsValid)
            {
                viewModel.IsAdmin = isAdmin;
                viewModel.BookOptions = BuildBookOptions();
                viewModel.CategoryOptions = BuildCategoryOptions();
                viewModel.PersonOptions = isAdmin ? BuildPersonOptions() : new List<SelectListItem>();
                return View(viewModel);
            }

            var borrow = BorrowViewModel.ToEntity(viewModel);
            borrow.CreatedBy = GetCurrentUserId();
            borrow.CreatedAt = DateTime.UtcNow;
            _borrowRepository.Add(borrow);

            book.AvailableCopies -= 1;
            _bookRepository.Update(book);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var borrows = _borrowRepository.GetAll();

            if (!User.IsInRole("Admin"))
            {
                var currentUser = await _userManager.FindByIdAsync(GetCurrentUserId());
                borrows = borrows.Where(b => b.PersonId == currentUser!.PersonId).ToList();
            }

            var viewModels = borrows.Select(b => BorrowListViewModel.FromEntity(b)).ToList();
            return View(viewModels);
        }

        [HttpPost]
        public async Task<IActionResult> Return(int id)
        {
            var borrow = _borrowRepository.GetById(id);
            if (borrow == null) return NotFound();

            if (!User.IsInRole("Admin"))
            {
                var currentUser = await _userManager.FindByIdAsync(GetCurrentUserId());
                if (currentUser?.PersonId == null || borrow.PersonId != currentUser.PersonId)
                {
                    return Forbid();
                }
            }

            borrow.ReturnDate = DateTime.UtcNow;
            borrow.UpdatedBy = GetCurrentUserId();
            borrow.UpdatedAt = DateTime.UtcNow;
            _borrowRepository.Update(borrow);

            var book = _bookRepository.GetById(borrow.BookId);
            if (book != null)
            {
                book.AvailableCopies += 1;
                _bookRepository.Update(book);
            }

            return RedirectToAction("Index");
        }
    }
}