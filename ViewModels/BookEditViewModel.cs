using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using PersonRegistrationMvc.Models;

namespace PersonRegistrationMvc.ViewModels
{
    public class BookEditViewModel
    {
        public int BookId { get; set; }

        [Required(ErrorMessage = "TitleRequired")]
        [StringLength(200, ErrorMessage = "TitleMaxLength")]
        public string Title { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "SelectCategoryRequired")]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "PublishedYearRequired")]
        [Range(1000, 2100, ErrorMessage = "PublishedYearRange")]
        public int PublishedYear { get; set; }

        [Required(ErrorMessage = "TotalCopiesRequired")]
        [Range(0, int.MaxValue, ErrorMessage = "TotalCopiesRange")]
        public int TotalCopies { get; set; }

        [Required(ErrorMessage = "AvailableCopiesRequired")]
        [Range(0, int.MaxValue, ErrorMessage = "AvailableCopiesRange")]
        public int AvailableCopies { get; set; }

        [ValidateNever]
        public List<SelectListItem> AuthorOptions { get; set; }

        public List<int> SelectedAuthorIds { get; set; } = new List<int>();

        [ValidateNever]
        public List<SelectListItem> CategoryOptions { get; set; }

        public static BookEditViewModel FromEntity(Book book)
        {
            return new BookEditViewModel
            {
                BookId = book.BookId,
                Title = book.Title,
                CategoryId = book.CategoryId,
                PublishedYear = book.PublishedYear,
                TotalCopies = book.TotalCopies,
                AvailableCopies = book.AvailableCopies
            };
        }

        public static void ApplyTo(BookEditViewModel viewModel, Book book)
        {
            book.Title = viewModel.Title;
            book.CategoryId = viewModel.CategoryId;
            book.PublishedYear = viewModel.PublishedYear;
            book.TotalCopies = viewModel.TotalCopies;
            book.AvailableCopies = viewModel.AvailableCopies;
        }
    }
}