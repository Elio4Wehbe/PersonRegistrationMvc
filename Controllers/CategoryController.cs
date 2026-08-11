using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using PersonRegistrationMvc.Repositories;
using PersonRegistrationMvc.ViewModels;

namespace PersonRegistrationMvc.Controllers
{
    [Authorize]
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly Microsoft.Extensions.Localization.IStringLocalizer<SharedResource> _localizer;

        public CategoryController(ICategoryRepository categoryRepository, Microsoft.Extensions.Localization.IStringLocalizer<SharedResource> localizer)
        {
            _categoryRepository = categoryRepository;
            _localizer = localizer;
        }

        private string GetCurrentUserId() => User.FindFirstValue(ClaimTypes.NameIdentifier);

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Add() => View();
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Add(CategoryAddViewModel viewModel)
        {
            if (!ModelState.IsValid) return View(viewModel);

            var category = CategoryAddViewModel.ToEntity(viewModel);
            category.CreatedBy = GetCurrentUserId();
            category.CreatedAt = DateTime.UtcNow;
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
            category.UpdatedBy = GetCurrentUserId();
            category.UpdatedAt = DateTime.UtcNow;

            _categoryRepository.Update(category);
            return RedirectToAction("Index");
        }


        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult DeleteConfirmed(int id)
        {
            if (_categoryRepository.HasBooks(id))
            {
                TempData["ErrorMessage"] = _localizer["CategoryNotDeletableMessage"].Value;
                return RedirectToAction("Index");
            }

            _categoryRepository.Delete(id);
            return RedirectToAction("Index");
        }
    }
}
