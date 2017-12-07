using DebtCardervice.Models.Debt;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DebtCardervice.Data
{
    public class StudentService : DbContext
    {
        public StudentService(DbContextOptions<LibraryService> options) : base(options)
        {
        }

        public DbSet<DebtCard> Concerts { get; set; }
        public DbSet<Library> Librarys { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DebtCard>().ToTable("Concerte");
            modelBuilder.Entity<Library>().ToTable("Library");
        }
    }
}
