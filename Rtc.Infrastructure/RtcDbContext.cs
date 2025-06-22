using Microsoft.EntityFrameworkCore;
using Rtc.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rtc.Infrastructure
{
    public  class RtcDbContext : DbContext
    {
        public RtcDbContext(DbContextOptions<RtcDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Currency>().HasKey(c => c.Idx);
            modelBuilder.Entity<Currency>()
                .HasIndex(c => c.CurrencyCode)
                .IsUnique();
        }

        public DbSet<CustomerBoardRate> CustomerBoardRates { get; set; }
        public  DbSet<CustomerBoardRateData> CustomerBoardRateDatas { get; set; }
        public  DbSet<Currency> Currencies { get; set; }

    }
}
