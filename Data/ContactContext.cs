using Microsoft.EntityFrameworkCore;
using Contact.Models;
namespace Contact.Data
{
    public class ContactContext : DbContext
    {
        public ContactContext(DbContextOptions<ContactContext> options) : base(options) { }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
        }
        public DbSet<contact> contact { get; set; }
        public DbSet<Departement> departments { get; set; }

    }
}
