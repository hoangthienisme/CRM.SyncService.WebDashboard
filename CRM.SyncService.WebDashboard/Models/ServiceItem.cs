using System.ComponentModel.DataAnnotations;

namespace CRM.SyncService.WebDashboard.Models
{
    public class ServiceItem
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required, Display(Name = "Service Name")]
        public string ServiceName { get; set; }

        [Display(Name = "Description")]
        public string? Description { get; set; }

        [Display(Name = "Price")]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }

        [Display(Name = "Category")]
        public string? Category { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
