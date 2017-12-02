using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArenaService.Models
{
    public class City
    {
        public int ID { get; set; }
        public string CityName { get; set; }
        public int CityPopulation { get; set; }

        //public virtual List<Arena> Arenas { get; set; }
    }
}
