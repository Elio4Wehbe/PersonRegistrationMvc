using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using PersonRegistrationMvc.Models;

namespace PersonRegistrationMvc.ViewModels
{
    public class BookAddViewModel
    {
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

        public static Book ToEntity(BookAddViewModel viewModel)
        {
            return new Book
            {
                Title = viewModel.Title,
                CategoryId = viewModel.CategoryId,
                PublishedYear = viewModel.PublishedYear,
                TotalCopies = viewModel.TotalCopies,
                AvailableCopies = viewModel.AvailableCopies
            };
        }
    }
}