using CRM.SyncService.WebDashboard.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CRM.SyncService.WebDashboard.Controllers
{
    public class DashboardController : Controller
    {
        private readonly AppDbContext _db;
        public DashboardController(AppDbContext db) => _db = db;

        public async Task<IActionResult> Index()
        {
            var model = new
            {
                TotalCustomers = await _db.Contacts.CountAsync(),
                MonthlyOrders = await _db.Orders.CountAsync(o => o.CreatedAt.Month == DateTime.UtcNow.Month),
                ActiveContracts = await _db.Contracts.CountAsync(c => c.Status == "Active"),
                MonthlyRevenue = await _db.Orders
                    .Where(o => o.CreatedAt.Month == DateTime.UtcNow.Month)
                    .SumAsync(o => (decimal?)o.TotalAmount) ?? 0,
                RecentOrders = await _db.Orders
                    .Include(o => o.Contact)
                    .OrderByDescending(o => o.CreatedAt)
                    .Take(5)
                    .ToListAsync()
            };

            return View(model);
        }
    }
}
