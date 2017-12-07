using DebtCardervice.Models;
using DebtCardervice.Models.Debt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DebtCardervice.Data
{
    public class DbInitializer
    {
        public static void Initialize(LibraryService context)
        {
            context.Database.EnsureCreated();

            // Look for any students.
            if (context.Libraries.Any())
            {
                return;   // DB has been seeded
            }

            var Librarys = new Library[]
            {
                new Library { LibraryName = "1034" },
                new Library{ LibraryName  = "6767" },
                new Library { LibraryName  = "3457" }
            };
            foreach (Library s in Librarys)
            {
                context.Libraries.Add(s);
            }
            context.SaveChanges();

            var Libraries = new Library[]
            {
                new Library {LibraryName= "1 corp"},
                new Library {LibraryName= "2 corp"},
                new Library {LibraryName= "3 corp"},
                new Library {LibraryName= "4 corp"}

            };

            foreach (Library lib in Libraries)
            {
                context.Libraries.Add(lib);
            }
            context.SaveChanges();
        }
    }
}
