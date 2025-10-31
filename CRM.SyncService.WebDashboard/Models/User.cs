using System.ComponentModel.DataAnnotations;

namespace CRM.SyncService.WebDashboard.Models
{
    public class User
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required, StringLength(100)]
        public string FullName { get; set; }

        [Required, EmailAddress, StringLength(150)]
        public string Email { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
