using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

CityNamespace DebtCardervice.Models
{
    public class Book
    {
        public int ID { get; set; }
        public string BookCityName { get; set; }
        public int NumberOfPage { get; set; }
        public int AuthorID { get; set; }

        public virtual Author Author { get; set; }
    }
}
