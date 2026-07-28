using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonRegistrationMvc.Repositories;
using PersonRegistrationMvc.ViewModels;


namespace PersonRegistrationMvc.Controllers
{
    [Authorize(Roles = "Admin")]
    public class PersonController : Controller
    {
        private readonly IPersonRepository _personRepository;
        private readonly IBorrowRepository _borrowRepository;

        public PersonController(IPersonRepository personRepository, IBorrowRepository borrowRepository)
        {
            _personRepository = personRepository;
            _borrowRepository = borrowRepository;
        }

        private string GetCurrentUser() => User.FindFirst("FullName")?.Value ?? User.Identity.Name;
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Register(PersonRegistrationViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            var person = PersonRegistrationViewModel.ToEntity(viewModel);
            person.CreatedBy = GetCurrentUser();
            person.CreatedAt = DateTime.Now;
            _personRepository.Add(person);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Index()
        {
            var people = _personRepository.GetAll();
            var viewModels = people.Select(p => PersonListViewModel.FromEntity(p)).ToList();
            return View(viewModels);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var person = _personRepository.GetById(id);
            if (person == null)
            {
                return NotFound();
            }

            var viewModel = PersonEditViewModel.FromEntity(person);
            return View(viewModel);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Edit(PersonEditViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            var person = _personRepository.GetById(viewModel.PersonId);
            if (person == null)
            {
                return NotFound();
            }

            PersonEditViewModel.ApplyTo(viewModel, person);
            person.UpdatedBy = GetCurrentUser();
            person.UpdatedAt = DateTime.Now;

            _personRepository.Update(person);
            return RedirectToAction("Index");
        }


        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult DeleteConfirmed(int id)
        {
            bool hasActiveBorrows = _borrowRepository.GetAll()
                .Any(b => b.PersonId == id && b.ReturnDate == null);

            if (hasActiveBorrows)
            {
                TempData["ErrorMessage"] = "This person cannot be archived because they have an active borrow.";
                return RedirectToAction("Index");
            }

            var person = _personRepository.GetById(id);
            if (person != null)
            {
                person.IsDeleted = true;
                person.ArchivedDate = DateTime.Now;
                _personRepository.Update(person);
            }
            return RedirectToAction("Index");
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Archive()
        {
            var people = _personRepository.GetAllIncludingDeleted()
                .Where(p => p.IsDeleted)
                .ToList();
            var viewModels = people.Select(p => PersonListViewModel.FromEntity(p)).ToList();
            return View(viewModels);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Restore(int id)
        {
            var person = _personRepository.GetById(id);
            if (person != null)
            {
                person.IsDeleted = false;
                person.ArchivedDate = null;
                _personRepository.Update(person);
            }
            return RedirectToAction("Archive");
        }
    }
}