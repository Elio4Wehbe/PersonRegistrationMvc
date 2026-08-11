using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using PersonRegistrationMvc.Models;

namespace PersonRegistrationMvc.ViewModels
{
    public class BorrowViewModel
    {
        [Required(ErrorMessage = "SelectPersonRequired")]
        [Range(1, int.MaxValue, ErrorMessage = "SelectPersonRequired")]
        public int PersonId { get; set; }

        [Required(ErrorMessage = "SelectBookRequired")]
        [Range(1, int.MaxValue, ErrorMessage = "SelectBookRequired")]
        public int BookId { get; set; }

        public DateTime BorrowDate { get; set; }
        public DateTime DueDate { get; set; }

        [ValidateNever]
        public bool IsAdmin { get; set; }

        [ValidateNever]
        public List<SelectListItem> PersonOptions { get; set; } = new();

        [ValidateNever]
        public List<SelectListItem> BookOptions { get; set; }

        [ValidateNever]
        public List<SelectListItem> CategoryOptions { get; set; }
        public int? SelectedCategoryId { get; set; }

        public static Borrow ToEntity(BorrowViewModel viewModel)
        {
            return new Borrow
            {
                PersonId = viewModel.PersonId,
                BookId = viewModel.BookId,
                BorrowDate = viewModel.BorrowDate,
                DueDate = viewModel.DueDate
            };
        }
    }
}