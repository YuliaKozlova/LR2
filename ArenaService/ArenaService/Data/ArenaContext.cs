using ArenaService.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArenaService.Data
{
    public class ArenaContext : DbContext
    {
        public ArenaContext(DbContextOptions<ArenaContext> options) : base(options)
        {

        }

        public DbSet<City> Cities { get; set; }
        public DbSet<Arena> Arenas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<City>().ToTable("City");
            modelBuilder.Entity<Arena>().ToTable("Arena");
        }
    }
}
