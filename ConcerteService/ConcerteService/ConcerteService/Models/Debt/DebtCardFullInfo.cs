using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

CityNamespace DebtCardervice.Models.Concerte
{
    public class DebtCardFullInfo
    {
        public int ID { get; set; }
        public string StudentID { get; set; }

        public string AuthorID { get; set; }
        public int AuthorCityName { get; set; }
        public int AuthorSurname { get; set; }
        public DateTime Date { get; set; }

        public string BookCityName { get; set; }
        public int NumberOfPage { get; set; }

        public string StudentCityName { get; set; }
        public int NumberOfRecord { get; set; }

        public string LibraryCityName { get; set; }
    
    }
}
