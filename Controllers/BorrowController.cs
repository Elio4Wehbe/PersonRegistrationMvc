using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PersonRegistrationMvc.Repositories;
using PersonRegistrationMvc.ViewModels;

namespace PersonRegistrationMvc.Controllers
{
    [Authorize(Roles = "Admin")]
    public class BorrowController : Controller
    {
        private readonly IBorrowRepository _borrowRepository;
        private readonly IPersonRepository _personRepository;
        private readonly IBookRepository _bookRepository;
        private readonly ICategoryRepository _categoryRepository;

        public BorrowController(
            IBorrowRepository borrowRepository,
            IPersonRepository personRepository,
            IBookRepository bookRepository,
            ICategoryRepository categoryRepository)
        {
            _borrowRepository = borrowRepository;
            _personRepository = personRepository;
            _bookRepository = bookRepository;
            _categoryRepository = categoryRepository;
        }

        private string GetCurrentUserId() => User.FindFirstValue(ClaimTypes.NameIdentifier);
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Borrow()
        {
            var viewModel = new BorrowViewModel
            {
                PersonOptions = _personRepository.GetAll()
                    .Select(p => new SelectListItem { Value = p.PersonId.ToString(), Text = p.FullName })
                    .ToList(),
                BookOptions = _bookRepository.GetAll()
                    .Select(b => new SelectListItem { Value = b.BookId.ToString(), Text = b.Title })
                    .ToList(),
                CategoryOptions = _categoryRepository.GetAll()
                    .Select(c => new SelectListItem { Value = c.CategoryId.ToString(), Text = c.Name })
                    .ToList()
            };

            return View(viewModel);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Borrow(BorrowViewModel viewModel)
        {
            viewModel.BorrowDate = DateTime.UtcNow;
            viewModel.DueDate = viewModel.BorrowDate.AddDays(14);

            var book = _bookRepository.GetById(viewModel.BookId);

            if (book == null)
            {
                ModelState.AddModelError("", "Selected book could not be found.");
            }
            else if (book.AvailableCopies < 1)
            {
                ModelState.AddModelError("", "This book is out of stock.");
            }

            if (ModelState.IsValid)
            {
                int activeBorrowCount = _borrowRepository.GetAll()
                    .Count(b => b.PersonId == viewModel.PersonId && b.ReturnDate == null);

                if (activeBorrowCount >= 3)
                {
                    ModelState.AddModelError("", "This person already has 3 active borrows.");
                }
            }

            if (!ModelState.IsValid)
            {
                viewModel.PersonOptions = _personRepository.GetAll()
                    .Select(p => new SelectListItem { Value = p.PersonId.ToString(), Text = p.FullName })
                    .ToList();
                viewModel.BookOptions = _bookRepository.GetAll()
                    .Select(b => new SelectListItem { Value = b.BookId.ToString(), Text = b.Title })
                    .ToList();
                viewModel.CategoryOptions = _categoryRepository.GetAll()
                    .Select(c => new SelectListItem { Value = c.CategoryId.ToString(), Text = c.Name })
                    .ToList();

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
        public IActionResult Index()
        {
            var borrows = _borrowRepository.GetAll();
            var viewModels = borrows.Select(b => BorrowListViewModel.FromEntity(b)).ToList();
            return View(viewModels);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Return(int id)
        {
            var borrow = _borrowRepository.GetById(id);
            if (borrow == null) return NotFound();

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