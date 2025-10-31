using CRM.SyncService.WebDashboard.Data;
using CRM.SyncService.WebDashboard.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CRM.SyncService.WebDashboard.Controllers
{
    public class ServiceItemController : Controller
    {
        private readonly AppDbContext _db;
        public ServiceItemController(AppDbContext db) => _db = db;

        public async Task<IActionResult> Index()
        {
            var services = await _db.Services
                .OrderByDescending(s => s.CreatedAt)
                .ToListAsync();
            return View(services);
        }

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ServiceItem service)
        {
            if (!ModelState.IsValid)
                return View(service);

            _db.Services.Add(service);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var service = await _db.Services.FindAsync(id);
            if (service == null) return NotFound();
            return View(service);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, ServiceItem service)
        {
            if (id != service.Id) return NotFound();
            if (!ModelState.IsValid)
                return View(service);

            _db.Services.Update(service);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            var service = await _db.Services.FindAsync(id);
            if (service == null) return NotFound();
            return View(service);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var service = await _db.Services.FindAsync(id);
            if (service != null)
            {
                _db.Services.Remove(service);
                await _db.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
