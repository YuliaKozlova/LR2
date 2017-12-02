using ConcerteService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConcerteService.Data
{
    public class DbInitializer
    {
        public static void Initialize(StudentContext context)
        {
            context.Database.EnsureCreated();

            if (context.Artists.Any())
            {
                return;   // DB has been seeded
            }

            var artists = new Student[]
            {
                new Student { ArtistName = "Marilyn Manson", LastFmRating = 10},
                new Student { ArtistName = "30 Seconds to Mars", LastFmRating = 7},
                new Student { ArtistName = "Rammstein", LastFmRating = 4},
                new Student { ArtistName = "The Beatles", LastFmRating = 1},
                new Student { ArtistName = "Suicide Silence", LastFmRating = 14},
                new Student { ArtistName = "Depeche mode", LastFmRating = 2},
                new Student { ArtistName = "Bullet for my valentine", LastFmRating = 15},
                new Student { ArtistName = "Frank Sinatra", LastFmRating = 3},
                new Student { ArtistName = "Elvis Presley", LastFmRating = 4},
                new Student { ArtistName = "Combichrist", LastFmRating = 8},
                new Student { ArtistName = "Devil sold his soul", LastFmRating = 9}
            };

            foreach (Student s in artists)
            {
                context.Artists.Add(s);
            }
            context.SaveChanges();
        }
    }
}
