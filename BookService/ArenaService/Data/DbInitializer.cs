using BookService.Models;
using BookService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookService.Data
{
    public class DbInitializer
    {
        public static void Initialize(BookContext context)
        {
            context.Database.EnsureCreated();

            if (context.Authors.Any())
            {
                return;   // DB has been seeded
            }

            var authors = new Author[]
            {
                new Author{AuthorName = "Alexander", AuthorSurname = "Pushkin", AuthorYearOfBirth=1799},
                new Author{AuthorName = "Mikhail", AuthorSurname = "Lermontov", AuthorYearOfBirth=1814},
                new Author{AuthorName = "Lev", AuthorSurname = "Tolstoy", AuthorYearOfBirth=1828}
            };
            foreach (Author c in authors)
            {
                context.Authors.Add(c);
            }
            context.SaveChanges();

            var books = new Book[]
            {
                new Book{ BookName = "Anna Karenina", NumberOfPage = 864, AuthorID  = 3},
                new Book{ BookName = "War and peace", NumberOfPage = 1200, AuthorID  = 3},
                new Book{ BookName = "Borodino", NumberOfPage = 250, AuthorID  = 2},
                new Book{ BookName = "Countess Ligovskoy", NumberOfPage = 340, AuthorID  = 2},
                new Book{ BookName = "Bronze horseman", NumberOfPage = 237, AuthorID  =1},
                new Book{ BookName = "The captain's daughter", NumberOfPage = 450, AuthorID  = 1},           
            };
            foreach (Book a in books)
            {
                context.Books.Add(a);
            }
            context.SaveChanges();
        }
    }
}
