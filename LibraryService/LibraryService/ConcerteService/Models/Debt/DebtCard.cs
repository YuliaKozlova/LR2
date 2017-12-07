using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DebtCardervice.Models.Debt
{
    public class DebtCard
    {
        public int ID { get; set; }
        public string StudentName { get; set; }
        public int NumberOfRecord { get; set; }
        public string AuthorName { get; set; }
        public int AuthorID { get; set; }
        public string AuthorSurname { get; set; }
        public string BookName { get; set; }
        public DateTime Date { get; set; }
        public int LibraryID { get; set; }

        public virtual Library Library { get; set; }
    }
}
