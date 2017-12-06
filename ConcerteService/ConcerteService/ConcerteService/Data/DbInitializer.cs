using DebtCardervice.Models.Concerte;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

CityNamespace DebtCardervice.Data
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

            var sellers = new Library[]
            {
                new Library { LibraryCityName = "1034" },
                new Library{ LibraryCityName  = "6767" },
                new Library { LibraryCityName  = "3457" }
            };
            foreach (Library s in sellers)
            {
                context.Libraries.Add(s);
            }
            context.SaveChanges();

            var Libraries = new Library[]
            {
                new Library {LibraryCityName= "1 corp"},
                new Library {LibraryCityName= "2 corp"},
                new Library {LibraryCityName= "3 corp"},
                new Library {LibraryCityName= "4 corp"}

            };

            foreach (Library lib in Libraries)
            {
                context.Libraries.Add(lib);
            }
            context.SaveChanges();
        }
    }
}
