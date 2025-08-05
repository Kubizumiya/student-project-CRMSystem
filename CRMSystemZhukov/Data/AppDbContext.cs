using System.Data.Entity;
using CRMSystemZhukov.Models;

namespace CRMSystemZhukov.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext() : base("name=DefaultConnection") { }

        public DbSet<Client> Clients { get; set; }
        public DbSet<Campaign> Campaigns { get; set; }
        public DbSet<Mailing> Mailings { get; set; }
        public DbSet<Deal> Deals { get; set; }
        public DbSet<Task> Tasks { get; set; }
    }
} 