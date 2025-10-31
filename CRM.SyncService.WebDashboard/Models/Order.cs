using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRM.SyncService.WebDashboard.Models
{
    public class Order
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [Display(Name = "Mã đơn hàng")]
        public string OrderCode { get; set; }

        [Display(Name = "Khách hàng")]
        public Guid ContactId { get; set; }

        [ForeignKey(nameof(ContactId))]
        public Contact Contact { get; set; }

        [Display(Name = "Tổng tiền")]
        public decimal TotalAmount { get; set; }

        [Display(Name = "Trạng thái")]
        public string Status { get; set; } = "Chờ xử lý"; // Hoàn thành, Đang xử lý,...

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
