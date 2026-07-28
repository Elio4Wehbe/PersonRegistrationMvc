// ViewModels/BorrowListViewModel.cs
using PersonRegistrationMvc.Models;

namespace PersonRegistrationMvc.ViewModels
{
    public class BorrowListViewModel
    {
        public int BorrowId { get; set; }
        public string PersonName { get; set; }
        public string BookTitle { get; set; }
        public DateTime BorrowDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public DateTime DueDate { get; set; }

        public bool IsOverdue => ReturnDate == null && DateTime.Now > DueDate;

        public static BorrowListViewModel FromEntity(Borrow borrow)
        {
            return new BorrowListViewModel
            {
                BorrowId = borrow.BorrowId,
                PersonName = borrow.Person.FullName,
                BookTitle = borrow.Book.Title,
                BorrowDate = borrow.BorrowDate,
                DueDate = borrow.DueDate,
                ReturnDate = borrow.ReturnDate
            };
        }
    }
}