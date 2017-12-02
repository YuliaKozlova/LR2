using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConcerteService.Models.Concerte
{
    public class ConcerteInfoFull
    {
        public int ID { get; set; }
        public string BrandName { get; set; }

        public string ShowName { get; set; }
        public uint TicketsNumber { get; set; }
        public uint Price { get; set; }
        public DateTime Date { get; set; }

        public string CityName { get; set; }
        public int CityPopulation { get; set; }

        public string ArenaName { get; set; }
        public int ArenaCapacity { get; set; }

        public string ArtistName { get; set; }
        public int LastFmRating { get; set; } 
    }
}
