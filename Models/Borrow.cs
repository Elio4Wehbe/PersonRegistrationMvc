namespace PersonRegistrationMvc.Models
{
    public class Borrow : AuditableEntity
    {
        public int BorrowId { get; set; }
        public DateTime BorrowDate { get; set; }
        public DateTime? ReturnDate { get; set; }


        public int PersonId { get; set; }
        public Person Person { get; set; }

        public int BookId { get; set; }
        public Book Book { get; set; }
        public DateTime DueDate { get; set; }
    }
}