using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonRegistrationMvc.Repositories;
using PersonRegistrationMvc.ViewModels;

namespace PersonRegistrationMvc.Controllers
{
    [Authorize]
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        private string GetCurrentUser() => User.FindFirst("FullName")?.Value ?? User.Identity.Name;

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Add() => View();
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Add(CategoryAddViewModel viewModel)
        {
            if (!ModelState.IsValid) return View(viewModel);

            var category = CategoryAddViewModel.ToEntity(viewModel);
            category.CreatedBy = GetCurrentUser();
            category.CreatedAt = DateTime.Now;
            _categoryRepository.Add(category);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Index()
        {
            var categories = _categoryRepository.GetAll();
            var viewModels = categories.Select(c => CategoryListViewModel.FromEntity(c)).ToList();
            return View(viewModels);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var category = _categoryRepository.GetById(id);
            if (category == null) return NotFound();

            var viewModel = CategoryEditViewModel.FromEntity(category);
            return View(viewModel);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Edit(CategoryEditViewModel viewModel)
        {
            if (!ModelState.IsValid) return View(viewModel);

            var category = _categoryRepository.GetById(viewModel.CategoryId);
            if (category == null) return NotFound();

            CategoryEditViewModel.ApplyTo(viewModel, category);
            category.UpdatedBy = GetCurrentUser();
            category.UpdatedAt = DateTime.Now;

            _categoryRepository.Update(category);
            return RedirectToAction("Index");
        }


        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult DeleteConfirmed(int id)
        {
            if (_categoryRepository.HasBooks(id))
            {
                TempData["ErrorMessage"] = "This category cannot be deleted because it still has books assigned to it.";
                return RedirectToAction("Index");
            }

            _categoryRepository.Delete(id);
            return RedirectToAction("Index");
        }
    }
}
