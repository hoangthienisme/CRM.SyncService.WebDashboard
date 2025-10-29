using System.ComponentModel.DataAnnotations;
namespace CRM.SyncService.WebDashboard.Models
{
    public class Contact
    {
        public Guid Id { get; set; } = Guid.NewGuid();


        [Required]
        [Display(Name = "Full Name")]
        public string ContactName { get; set; }


        [Display(Name = "Phone number")]
        public string Phone { get; set; }


        [EmailAddress]
        public string Email { get; set; }


        public string Status { get; set; }
        public string Source { get; set; }


        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}