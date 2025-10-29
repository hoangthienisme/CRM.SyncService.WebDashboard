using CRM.SyncService.WebDashboard.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CRM.SyncService.WebDashboard.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContactsApiController : ControllerBase
    {
        private readonly AppDbContext _db;

        public ContactsApiController(AppDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var list = await _db.Contacts
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();

            return Ok(list);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var contact = await _db.Contacts.FindAsync(id);
            if (contact == null) return NotFound();
            return Ok(contact);
        }
    }

}
