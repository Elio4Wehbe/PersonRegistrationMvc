using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using PersonRegistrationMvc.Models;

namespace PersonRegistrationMvc.ViewModels
{
    public class BorrowViewModel
    {
        [Range(1, int.MaxValue, ErrorMessage = "Please select a person.")]
        public int PersonId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Please select a book.")]
        public int BookId { get; set; }

        public DateTime BorrowDate { get; set; }
        public DateTime DueDate { get; set; }

        [ValidateNever]
        public List<SelectListItem> PersonOptions { get; set; }

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