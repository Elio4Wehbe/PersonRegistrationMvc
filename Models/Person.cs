namespace PersonRegistrationMvc.Models
{
    public class Person : AuditableEntity
    {
        public int PersonId { get; set; }
        public string FullName { get; set; }
        public int Age { get; set; }
        public Gender Gender { get; set; }
        public string Major { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? ArchivedDate { get; set; }

    }
}