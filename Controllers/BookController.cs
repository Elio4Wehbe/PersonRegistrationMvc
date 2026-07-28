using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PersonRegistrationMvc.Repositories;
using PersonRegistrationMvc.ViewModels;

namespace PersonRegistrationMvc.Controllers
{
    [Authorize]
    public class BookController : Controller
    {
        private readonly IBookRepository _bookRepository;
        private readonly IBorrowRepository _borrowRepository;
        private readonly IAuthorRepository _authorRepository;
        private readonly ICategoryRepository _categoryRepository;

        public BookController(
            IBookRepository bookRepository,
            IBorrowRepository borrowRepository,
            IAuthorRepository authorRepository,
            ICategoryRepository categoryRepository)
        {
            _bookRepository = bookRepository;
            _borrowRepository = borrowRepository;
            _authorRepository = authorRepository;
            _categoryRepository = categoryRepository;
        }

        private List<SelectListItem> BuildAuthorOptions() =>
            _authorRepository.GetAll().Select(a => new SelectListItem { Value = a.AuthorId.ToString(), Text = a.Name }).ToList();

        private List<SelectListItem> BuildCategoryOptions() =>
            _categoryRepository.GetAll().Select(c => new SelectListItem { Value = c.CategoryId.ToString(), Text = c.Name }).ToList();

        private string GetCurrentUserId() => User.FindFirstValue(ClaimTypes.NameIdentifier);

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Add()
        {
            var viewModel = new BookAddViewModel
            {
                AuthorOptions = BuildAuthorOptions(),
                CategoryOptions = BuildCategoryOptions()
            };
            return View(viewModel);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Add(BookAddViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.AuthorOptions = BuildAuthorOptions();
                viewModel.CategoryOptions = BuildCategoryOptions();
                return View(viewModel);
            }

            var book = BookAddViewModel.ToEntity(viewModel);
            book.CreatedBy = GetCurrentUserId();
            book.CreatedAt = DateTime.UtcNow;
            _bookRepository.Add(book);
            _bookRepository.SetAuthorsForBook(book.BookId, viewModel.SelectedAuthorIds);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Index()
        {
            var books = _bookRepository.GetAll();
            var viewModels = books.Select(b => BookListViewModel.FromEntity(b)).ToList();
            return View(viewModels);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var book = _bookRepository.GetById(id);
            if (book == null) return NotFound();

            var viewModel = BookEditViewModel.FromEntity(book);
            viewModel.AuthorOptions = BuildAuthorOptions();
            viewModel.SelectedAuthorIds = book.BookAuthors.Select(ba => ba.AuthorId).ToList();
            viewModel.CategoryOptions = BuildCategoryOptions();
            return View(viewModel);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Edit(BookEditViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.AuthorOptions = BuildAuthorOptions();
                viewModel.CategoryOptions = BuildCategoryOptions();
                return View(viewModel);
            }

            var book = _bookRepository.GetById(viewModel.BookId);
            if (book == null) return NotFound();

            BookEditViewModel.ApplyTo(viewModel, book);
            book.UpdatedBy = GetCurrentUserId();
            book.UpdatedAt = DateTime.UtcNow;

            _bookRepository.Update(book);
            _bookRepository.SetAuthorsForBook(book.BookId, viewModel.SelectedAuthorIds);
            return RedirectToAction("Index");
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Archive()
        {
            var books = _bookRepository.GetAllIncludingDeleted().Where(b => b.IsDeleted).ToList();
            var viewModels = books.Select(b => BookListViewModel.FromEntity(b)).ToList();
            return View(viewModels);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Restore(int id)
        {
            var book = _bookRepository.GetById(id);
            if (book != null)
            {
                book.IsDeleted = false;
                book.ArchivedDate = null;
                book.UpdatedBy = GetCurrentUserId();
                book.UpdatedAt = DateTime.UtcNow;
                _bookRepository.Update(book);
            }
            return RedirectToAction("Archive");
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult DeleteConfirmed(int id)
        {
            bool hasActiveBorrows = _borrowRepository.GetAll()
                .Any(b => b.BookId == id && b.ReturnDate == null);

            if (hasActiveBorrows)
            {
                TempData["ErrorMessage"] = "This book cannot be archived because it has active borrows.";
                return RedirectToAction("Index");
            }

            var book = _bookRepository.GetById(id);
            if (book != null)
            {
                book.IsDeleted = true;
                book.ArchivedDate = DateTime.UtcNow;
                book.UpdatedBy = GetCurrentUserId();
                book.UpdatedAt = DateTime.UtcNow;
                _bookRepository.Update(book);
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public JsonResult GetBooksByCategory(int categoryId)
        {
            var books = _bookRepository.GetAll()
                .Where(b => b.CategoryId == categoryId)
                .Select(b => new { value = b.BookId, text = b.Title })
                .ToList();
            return Json(books);
        }

        [HttpGet]
        public JsonResult GetBookDetails(int id)
        {
            var book = _bookRepository.GetById(id);
            if (book == null) return Json(null);
            return Json(BookDetailViewModel.FromEntity(book));
        }
    }
}