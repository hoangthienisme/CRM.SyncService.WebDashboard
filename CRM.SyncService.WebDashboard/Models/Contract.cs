using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRM.SyncService.WebDashboard.Models
{
    public class Contract
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required, Display(Name = "Contract Code")]
        public string ContractCode { get; set; }

        [Required, Display(Name = "Contact")]
        public Guid ContactId { get; set; }

        [ForeignKey(nameof(ContactId))]
        public Contact? Contact { get; set; }

        [Display(Name = "Service")]
        public Guid? ServiceId { get; set; }

        [ForeignKey(nameof(ServiceId))]
        public ServiceItem? Service { get; set; }

        [Display(Name = "Total Amount")]
        [DataType(DataType.Currency)]
        public decimal TotalAmount { get; set; }

        [Display(Name = "Start Date")]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [Display(Name = "End Date")]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        [Display(Name = "Status")]
        public string Status { get; set; } = "Active";
    }
}
