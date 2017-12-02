using ArenaService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArenaService.Data
{
    public class DbInitializer
    {
        public static void Initialize(ArenaContext context)
        {
            context.Database.EnsureCreated();

            if (context.Cities.Any())
            {
                return;   // DB has been seeded
            }

            var citys = new City[]
            {
                new City{CityName = "Moscow", CityPopulation = 12000000},
                new City{CityName = "Berlin", CityPopulation = 3500000},
                new City{CityName = "London", CityPopulation = 9000000}
            };
            foreach (City c in citys)
            {
                context.Cities.Add(c);
            }
            context.SaveChanges();

            var arenas = new Arena[]
            {
                new Arena{ ArenaName = "Crocus City Hall", CityID = 1, Capacity = 6000},
                new Arena{ ArenaName = "Olimpiyskiy", CityID = 1, Capacity = 10000},
                new Arena{ ArenaName = "Vegas City Hall", CityID = 1, Capacity = 6000},
                new Arena{ ArenaName = "Wembley Arena", CityID = 3, Capacity = 4000},
                new Arena{ ArenaName = "Brixton Academy", CityID = 3, Capacity = 10000},
                new Arena{ ArenaName = "Mercedes-Benz Arena", CityID = 2, Capacity = 6000},
                new Arena{ ArenaName = "Olympiastadion Berlin", CityID = 2, Capacity = 9000},
                new Arena{ ArenaName = "Rock am Ring", CityID = 2, Capacity = 15000},
                new Arena{ ArenaName = "Vova Arena", CityID = 1, Capacity = 3000},
                new Arena{ ArenaName = "Natasha Arena", CityID = 1, Capacity = 3000},
                new Arena{ ArenaName = "Big Arena", CityID = 2, Capacity = 20000},
                new Arena{ ArenaName = "Hell Fire Arena", CityID = 3, Capacity = 10000},
            };
            foreach (Arena a in arenas)
            {
                context.Arenas.Add(a);
            }
            context.SaveChanges();
        }
    }
}
