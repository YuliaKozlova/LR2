using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DebtCardervice.Models.Debt
{
    public class DebtCardFullInfo
    {
        public int ID { get; set; }
        public string StudentID { get; set; }

        public int AuthorID { get; set; }
        public string AuthorName { get; set; }
        public string AuthorSurname { get; set; }
        public DateTime Date { get; set; }

        public string BookName { get; set; }
        public int NumberOfPage { get; set; }

        public string StudentName { get; set; }
        public int NumberOfRecord { get; set; }

        public string LibraryName { get; set; }
    
    }
}
