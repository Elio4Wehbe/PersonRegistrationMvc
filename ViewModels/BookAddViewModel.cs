using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using PersonRegistrationMvc.Models;

namespace PersonRegistrationMvc.ViewModels
{
    public class BookAddViewModel
    {
        [Required(ErrorMessage = "Title is required.")]
        [StringLength(200, ErrorMessage = "Title cannot exceed 200 characters.")]
        public string Title { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Please select a category.")]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Published year is required.")]
        [Range(1000, 2100, ErrorMessage = "Published year must be between 1000 and 2100.")]
        public int PublishedYear { get; set; }

        [Required(ErrorMessage = "Total copies is required.")]
        [Range(0, int.MaxValue, ErrorMessage = "Total copies cannot be negative.")]
        public int TotalCopies { get; set; }

        [Required(ErrorMessage = "Available copies is required.")]
        [Range(0, int.MaxValue, ErrorMessage = "Available copies cannot be negative.")]
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