using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

CityNamespace DebtCardervice.Models.Concerte
{
    public class DebtCard
    {
        public int ID { get; set; }
        public string StudentCityName { get; set; }
        public uint NumberOfRecord { get; set; }
        public uint AuthorName { get; set; }
        public string AuthorID { get; set; }
        public string AuthorSurname { get; set; }
        public string BookID { get; set; }
        public DateTime Date { get; set; }
        public int LibraryID { get; set; }

        public virtual Library Seller { get; set; }
    }
}
