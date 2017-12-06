using DebtCardervice.Models.Concerte;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

CityNamespace DebtCardervice.Data
{
    public class LibraryService : DbContext
    {
        public LibraryService(DbContextOptions<LibraryService> options) : base(options)
        {
        }

        public DbSet<DebtCard> DebtCard { get; set; }
        public DbSet<Library> Libraries { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DebtCard>().ToTable("DebtCard");
            modelBuilder.Entity<Library>().ToTable("Library");
        }
    }
}
