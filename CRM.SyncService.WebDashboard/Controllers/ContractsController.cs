using CRM.SyncService.WebDashboard.Data;
using CRM.SyncService.WebDashboard.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CRM.SyncService.WebDashboard.Controllers
{
    public class ContractsController : Controller
    {
        private readonly AppDbContext _db;
        public ContractsController(AppDbContext db) => _db = db;

        public async Task<IActionResult> Index()
        {
            var list = await _db.Contracts
                .Include(c => c.Contact)
                .Include(c => c.Service)
                .OrderByDescending(c => c.StartDate)
                .ToListAsync();
            return View(list);
        }

        public IActionResult Create()
        {
            ViewBag.Contacts = new SelectList(_db.Contacts, "Id", "ContactName");
            ViewBag.Services = new SelectList(_db.Services, "Id", "ServiceName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Contract contract)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Contacts = new SelectList(_db.Contacts, "Id", "ContactName", contract.ContactId);
                ViewBag.Services = new SelectList(_db.Services, "Id", "ServiceName", contract.ServiceId);
                return View(contract);
            }

            contract.ContractCode ??= $"CT-{DateTime.UtcNow.Ticks}";
            _db.Contracts.Add(contract);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var contract = await _db.Contracts.FindAsync(id);
            if (contract == null) return NotFound();
            ViewBag.Contacts = new SelectList(_db.Contacts, "Id", "ContactName", contract.ContactId);
            ViewBag.Services = new SelectList(_db.Services, "Id", "ServiceName", contract.ServiceId);
            return View(contract);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, Contract contract)
        {
            if (id != contract.Id) return NotFound();
            if (!ModelState.IsValid)
            {
                ViewBag.Contacts = new SelectList(_db.Contacts, "Id", "ContactName", contract.ContactId);
                ViewBag.Services = new SelectList(_db.Services, "Id", "ServiceName", contract.ServiceId);
                return View(contract);
            }

            _db.Contracts.Update(contract);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            var contract = await _db.Contracts
                .Include(c => c.Contact)
                .Include(c => c.Service)
                .FirstOrDefaultAsync(c => c.Id == id);
            if (contract == null) return NotFound();
            return View(contract);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var contract = await _db.Contracts.FindAsync(id);
            if (contract != null)
            {
                _db.Contracts.Remove(contract);
                await _db.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
