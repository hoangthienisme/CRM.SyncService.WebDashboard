using Microsoft.AspNetCore.Mvc;
using CRM.SyncService.WebDashboard.Data;
using CRM.SyncService.WebDashboard.Models;
using Microsoft.EntityFrameworkCore;
using CRM.SyncService.WebDashboard.Services;

namespace CRM.SyncService.WebDashboard.Controllers
{
    public class ContactsController : Controller
    {   //private readonly GoogleSheetService _sheet; 
        //private readonly TelegramService _tg;
        private readonly AppDbContext _db;
        private readonly ClickUpService _clickUp;

        public ContactsController(AppDbContext db, ClickUpService clickUp/*GoogleSheetService sheet,*//* TelegramService tg*/)
        {
            _db = db;
            _clickUp = clickUp;
            //_sheet = sheet; 
            //_tg = tg;
        }
        public async Task<IActionResult> Index()
        {
            var list = await _db.Contacts
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();
            return View(list);
        }
        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Contact contact, bool syncToClickUp = false)
        {
            if (!ModelState.IsValid)
                return View(contact);

            contact.CreatedAt = DateTime.UtcNow;

            _db.Contacts.Add(contact);
            await _db.SaveChangesAsync();

            if (syncToClickUp)
                await SyncContact(contact);

            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public async Task<IActionResult> Sync(Guid id)
        {
            var contact = await _db.Contacts.FindAsync(id);
            if (contact == null)
                return NotFound("Contact not found");

            var result = await SyncContact(contact);
            if (!result.success)
                return StatusCode(500, result.error);

            return Ok(new { status = "ok" });
        }
        private async Task<(bool success, string? error)> SyncContact(Contact contact)
        {
            try
            {
                await _clickUp.PushContactAsync(contact);
                // await _sheet.UpdateContactAsync(contact); 
                //await _tg.SendLogAsync($" Sync success: {contact.ContactName}");
                return (true, null);
            }
            catch (Exception ex)
            {  //await _tg.SendLogAsync($" Sync failed: {ex.Message}");
                return (false, ex.Message);
            }
        }
        public async Task<IActionResult> Edit(Guid id)
        {
            var contact = await _db.Contacts.FindAsync(id);
            if (contact == null)
                return NotFound();

            return View(contact);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, Contact contact)
        {
            if (id != contact.Id)
                return BadRequest();

            if (!ModelState.IsValid)
                return View(contact);

            _db.Contacts.Update(contact);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(Guid id)
        {
            var contact = await _db.Contacts.FindAsync(id);
            if (contact == null)
                return NotFound();

            return View(contact);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var contact = await _db.Contacts.FindAsync(id);
            if (contact == null)
                return NotFound();

            _db.Contacts.Remove(contact);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
