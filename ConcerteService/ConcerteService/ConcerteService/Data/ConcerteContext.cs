using ConcerteService.Models.Concerte;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConcerteService.Data
{
    public class ConcerteContext : DbContext
    {
        public ConcerteContext(DbContextOptions<ConcerteContext> options) : base(options)
        {
        }

        public DbSet<Concerte> Concerts { get; set; }
        public DbSet<Seller> Sellers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Concerte>().ToTable("Concerte");
            modelBuilder.Entity<Seller>().ToTable("Seller");
        }
    }
}
