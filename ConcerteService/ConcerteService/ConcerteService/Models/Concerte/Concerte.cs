using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConcerteService.Models.Concerte
{
    public class Concerte
    {
        public int ID { get; set; }
        public string ShowName { get; set; }
        public uint TicketsNumber { get; set; }
        public uint Price { get; set; }
        public string CityName { get; set; }
        public string ArenaName { get; set; }
        public string ArtistName { get; set; }
        public DateTime Date { get; set; }
        public int SellerID { get; set; }

        public virtual Seller Seller { get; set; }
    }
}
