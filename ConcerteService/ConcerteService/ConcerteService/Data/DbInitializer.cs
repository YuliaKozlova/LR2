using ConcerteService.Models.Concerte;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConcerteService.Data
{
    public class DbInitializer
    {
        public static void Initialize(ConcerteContext context)
        {
            context.Database.EnsureCreated();

            // Look for any students.
            if (context.Sellers.Any())
            {
                return;   // DB has been seeded
            }

            var sellers = new Seller[]
            {
                new Seller{ BrandName = "Big Billet" },
                new Seller{ BrandName = "Afisha" },
                new Seller { BrandName = "Ozone" }
            };
            foreach (Seller s in sellers)
            {
                context.Sellers.Add(s);
            }
            context.SaveChanges();

            var concertes = new Concerte[]
            {
                new Concerte{ ArenaName = "Crocus City Hall", ArtistName = "Marilyn Manson", CityName = "Moscow", Date=DateTime.Parse("2015-09-01"), SellerID = 1, ShowName = "BigSHow", TicketsNumber = 1000, Price = 2000},
                new Concerte{ ArenaName = "Brixton Academy", ArtistName = "Marilyn Manson", CityName = "London", Date=DateTime.Parse("2015-10-01"), SellerID = 2, ShowName = "Hello, London", TicketsNumber = 2000, Price = 4000},
                new Concerte{ ArenaName = "Mercedes-Benz Arena", ArtistName = "Marilyn Manson", CityName = "Berlin", Date=DateTime.Parse("2015-11-01"), SellerID = 2, ShowName = "Hello, Berlin", TicketsNumber = 3000, Price = 6000},
                new Concerte{ ArenaName = "Crocus City Hall", ArtistName = "Depeche mode", CityName = "Moscow", Date=DateTime.Parse("2016-09-01"), SellerID = 1, ShowName = "BigSHow", TicketsNumber = 1000, Price = 9000},
                new Concerte{ ArenaName = "Brixton Academy", ArtistName = "Depeche mode", CityName = "London", Date=DateTime.Parse("2016-10-01"), SellerID = 2, ShowName = "Hello, Hello", TicketsNumber = 2000, Price = 11000},
                new Concerte{ ArenaName = "Mercedes-Benz Arena", ArtistName = "Depeche mode", CityName = "Berlin", Date=DateTime.Parse("2016-11-01"), SellerID = 2, ShowName = "Hello, Berlin", TicketsNumber = 3000, Price = 1000},
                new Concerte{ ArenaName = "Olimpiyskiy", ArtistName = "30 Seconds to Mars", CityName = "Moscow", Date=DateTime.Parse("2016-02-01"), SellerID = 1, ShowName = "BigGiG", TicketsNumber = 3000, Price = 22000},
                new Concerte{ ArenaName = "Brixton Academy", ArtistName = "30 Seconds to Mars", CityName = "London", Date=DateTime.Parse("2016-03-01"), SellerID = 2, ShowName = "Hello,Good Bye", TicketsNumber = 5000, Price = 3100},
                new Concerte{ ArenaName = "Mercedes-Benz Arena", ArtistName = "30 Seconds to Mars", CityName = "Berlin", Date=DateTime.Parse("2016-04-01"), SellerID = 2, ShowName = "Hello, WORLD", TicketsNumber = 6000, Price = 2400},
                new Concerte{ ArenaName = "Vova Arena", ArtistName = "Frank Sinatra", CityName = "Moscow", Date=DateTime.Parse("2017-09-01"), SellerID = 3, ShowName = "VovaSHow", TicketsNumber = 1000, Price = 2600},
                new Concerte{ ArenaName = "Vova Arena", ArtistName = "Frank Sinatra", CityName = "Moscow", Date=DateTime.Parse("2017-10-01"), SellerID = 3, ShowName = "VovaSHow2", TicketsNumber = 1000, Price = 3500},

                new Concerte{ ArenaName = "Mercedes-Benz Arena", ArtistName = "30 Seconds to Mars", CityName = "Berlin", Date=DateTime.Parse("2016-04-01"), SellerID = 2, ShowName = "Hello, WORLD", TicketsNumber = 99000, Price = 44000},
                new Concerte{ ArenaName = "Vova Arena", ArtistName = "Frank Sinatra", CityName = "Berlin", Date=DateTime.Parse("2017-09-01"), SellerID = 3, ShowName = "VovaSHow", TicketsNumber = 1000, Price = 4500},
                new Concerte{ ArenaName = "Mercedes-Benz Arena", ArtistName = "Eminem", CityName = "Berlin", Date=DateTime.Parse("2017-09-01"), SellerID = 3, ShowName = "VovaSHow", TicketsNumber = 1000, Price = 3000},
                new Concerte{ ArenaName = "Trash Arena", ArtistName = "Eminem", CityName = "Berlin", Date=DateTime.Parse("2017-09-01"), SellerID = 3, ShowName = "VovaSHow", TicketsNumber = 1000, Price = 8000}
            };

            foreach (Concerte c in concertes)
            {
                context.Concerts.Add(c);
            }
            context.SaveChanges();
        }
    }
}
