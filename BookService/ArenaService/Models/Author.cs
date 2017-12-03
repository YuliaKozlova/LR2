using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookService.Models
{
    public class Author
    {
        public int ID { get; set; }
        public string AuthorName { get; set; }
        public string AuthorSurname { get; set; }
        public int AuthorYearOfBirth { get; set; }

        //public virtual List<Author> Book { get; set; }
    }
}
