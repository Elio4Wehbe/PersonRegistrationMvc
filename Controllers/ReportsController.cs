using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonRegistrationMvc.Repositories;
using PersonRegistrationMvc.ViewModels;

namespace PersonRegistrationMvc.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ReportsController : Controller
    {
        private readonly IBookRepository _bookRepository;
        private readonly IPersonRepository _personRepository;
        private readonly IBorrowRepository _borrowRepository;

        public ReportsController(
            IBookRepository bookRepository,
            IPersonRepository personRepository,
            IBorrowRepository borrowRepository)
        {
            _bookRepository = bookRepository;
            _personRepository = personRepository;
            _borrowRepository = borrowRepository;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var viewModel = ReportsViewModel.Build(
                _bookRepository.GetAll(),
                _personRepository.GetAll(),
                _borrowRepository.GetAll());

            return View(viewModel);
        }
    }
}