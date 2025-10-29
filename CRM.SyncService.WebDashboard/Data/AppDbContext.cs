using Microsoft.EntityFrameworkCore;
using CRM.SyncService.WebDashboard.Models;
using System.Collections.Generic;


namespace CRM.SyncService.WebDashboard.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }


        public DbSet<Contact> Contacts { get; set; }
    }
}