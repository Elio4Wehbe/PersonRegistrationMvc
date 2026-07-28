namespace PersonRegistrationMvc.ViewModels
{
    public class PersonDetailViewModel
    {
        public int PersonId { get; set; }
        public string FullName { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }
        public string Major { get; set; }
        public int ActiveBorrowCount { get; set; }
        public List<string> CurrentlyBorrowedBooks { get; set; }
    }
}
