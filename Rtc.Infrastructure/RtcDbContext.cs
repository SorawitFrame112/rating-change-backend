using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore; 
using Rtc.Domain.Entities;

namespace Rtc.Infrastructure
{
    public class RtcDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        public RtcDbContext(DbContextOptions<RtcDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Currency>().HasKey(c => c.Idx);
            modelBuilder.Entity<Currency>().HasIndex(c => c.CurrencyCode).IsUnique();
            modelBuilder.Entity<Currency>().Property(c => c.CurrencyCode).IsRequired().HasMaxLength(50);
            modelBuilder.Entity<Currency>().Property(c => c.CurrencyName).IsRequired().HasMaxLength(100);

        }
            public DbSet<Currency> Currencies { get; set; }
    }
}
