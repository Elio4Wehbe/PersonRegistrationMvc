using Microsoft.AspNetCore.Identity;

namespace PersonRegistrationMvc.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public int? PersonId { get; set; }
        public Person? Person { get; set; }
    }
}