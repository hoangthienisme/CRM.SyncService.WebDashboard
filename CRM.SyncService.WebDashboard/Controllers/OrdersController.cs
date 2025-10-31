using CRM.SyncService.WebDashboard.Data;
using CRM.SyncService.WebDashboard.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CRM.SyncService.WebDashboard.Controllers
{
    public class OrdersController : Controller
    {
        private readonly AppDbContext _db;
        public OrdersController(AppDbContext db) => _db = db;

        public async Task<IActionResult> Index()
        {
            var orders = await _db.Orders.Include(o => o.Contact).ToListAsync();
            return View(orders);
        }

        public IActionResult Create()
        {
            ViewBag.Contacts = new SelectList(_db.Contacts, "Id", "ContactName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Order order)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Contacts = new SelectList(_db.Contacts, "Id", "ContactName", order.ContactId);
                return View(order);
            }

            order.OrderCode ??= $"ORD-{DateTime.UtcNow.Ticks}";
            _db.Add(order);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var order = await _db.Orders.FindAsync(id);
            if (order == null) return NotFound();
            ViewBag.Contacts = new SelectList(_db.Contacts, "Id", "ContactName", order.ContactId);
            return View(order);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, Order order)
        {
            if (id != order.Id) return NotFound();
            if (!ModelState.IsValid)
            {
                ViewBag.Contacts = new SelectList(_db.Contacts, "Id", "ContactName", order.ContactId);
                return View(order);
            }

            _db.Update(order);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            var order = await _db.Orders.Include(o => o.Contact).FirstOrDefaultAsync(o => o.Id == id);
            if (order == null) return NotFound();
            return View(order);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var order = await _db.Orders.FindAsync(id);
            if (order != null)
            {
                _db.Orders.Remove(order);
                await _db.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
