using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using PersonRegistrationMvc.Repositories;
using PersonRegistrationMvc.ViewModels;

namespace PersonRegistrationMvc.Controllers
{
    [Authorize]
    public class AuthorController : Controller
    {
        private readonly IAuthorRepository _authorRepository;
        private readonly IBookRepository _bookRepository;

        public AuthorController(IAuthorRepository authorRepository, IBookRepository bookRepository)
        {
            _authorRepository = authorRepository;
            _bookRepository = bookRepository;
        }

        private string GetCurrentUserId() => User.FindFirstValue(ClaimTypes.NameIdentifier);

        [HttpGet]
        public JsonResult GetAuthorDetails(int id)
        {
            var author = _authorRepository.GetById(id);
            if (author == null) return Json(null);

            var books = _bookRepository.GetByAuthorId(id);
            return Json(AuthorDetailViewModel.FromEntity(author, books));
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Add(AuthorAddViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            var author = AuthorAddViewModel.ToEntity(viewModel);
            author.CreatedBy = GetCurrentUserId();
            author.CreatedAt = DateTime.UtcNow;
            _authorRepository.Add(author);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Index()
        {
            var authors = _authorRepository.GetAll();
            var viewModels = authors.Select(a => AuthorListViewModel.FromEntity(a)).ToList();
            return View(viewModels);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var author = _authorRepository.GetById(id);
            if (author == null) return NotFound();

            var viewModel = AuthorEditViewModel.FromEntity(author);
            return View(viewModel);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Edit(AuthorEditViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            var author = _authorRepository.GetById(viewModel.AuthorId);
            if (author == null) return NotFound();

            AuthorEditViewModel.ApplyTo(viewModel, author);
            author.UpdatedBy = GetCurrentUserId();
            author.UpdatedAt = DateTime.UtcNow;

            _authorRepository.Update(author);
            return RedirectToAction("Index");
        }


        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult DeleteConfirmed(int id)
        {
            if (_authorRepository.IsLinkedToBooks(id))
            {
                TempData["ErrorMessage"] = "This author cannot be deleted because they are linked to one or more books.";
                return RedirectToAction("Index");
            }

            _authorRepository.Delete(id);
            return RedirectToAction("Index");
        }
    }
}