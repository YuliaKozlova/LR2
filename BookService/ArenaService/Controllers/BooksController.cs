using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookService.Data;
using BookService.Models;
using ReflectionIT.Mvc.Paging;
using BookService.Models.JsonBindings;


namespace Bookservice.Controllers
{
    [Produces("application/json")]
    [Route("api/Books")]
    public class BooksController : Controller
    {
        const int StringsPerPage = 10;


        private readonly BookContext _context;


        public BooksController(BookContext context)
        {
            _context = context;
        }


        // GET: api/Books/page/{id}
        [HttpGet]
        [Route("page/{page}")]
        public List<Book> GetBooks([FromRoute] int page = 1)
        {
            var qry = _context.Books.OrderBy(p => p.BookName);
            //foreach (Book a in qry)
            //{
            //    _context.Entry(a).Navigation("Book").Load();
            //}

            PagingList<Book> BooksList;
            if (page != 0)
            {
                
                BooksList = PagingList.Create(qry, StringsPerPage, page);
            }
            else
            {
                BooksList = PagingList.Create(qry, _context.Books.Count() + 1, 1);
            }

            return BooksList.ToList();
        }


        [HttpGet]
        public List<Book> GetBooksAll()
        {
            var qry = _context.Books.OrderBy(p => p.BookName);
            foreach (Book a in qry)
            {
                _context.Entry(a).Navigation("Author").Load();
            }

            PagingList<Book> BooksList;

            BooksList = PagingList.Create(qry, _context.Books.Count() + 1, 1);

            return BooksList.ToList();
        }


        //[HttpGet]
        //public IEnumerable<Book> GetBooksAll()
        //{
        //    IEnumerable<Book> Books = _context.Books;
        //    foreach (Book a in Books)
        //    {
        //        _context.Entry(a).Navigation("Book").Load();
        //    }

        //    return Books;
        //}


        // GET: api/Books/5

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBook([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var Book = await _context.Books.SingleOrDefaultAsync(m => m.ID == id);

            if (Book == null)
            {
                return NotFound();
            }

            return Ok(Book);
        }


        // PUT: api/Books/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBook([FromRoute] int id, [FromBody] Book Book)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != Book.ID)
            {
                return BadRequest();
            }

            _context.Entry(Book).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return Accepted(Book);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            //return NoContent();
        }


        // POST: api/Books
        [HttpPost]
        public async Task<IActionResult> PostBook([FromBody] Book Book)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Books.Add(Book);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBook", new { id = Book.ID }, Book);
        }


        // DELETE: api/Books/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var Book = await _context.Books.SingleOrDefaultAsync(m => m.ID == id);
            if (Book == null)
            {
                return NotFound();
            }

            _context.Books.Remove(Book);
            await _context.SaveChangesAsync();

            return Ok(Book);
        }

        private bool BookExists(int id)
        {
            return _context.Books.Any(e => e.ID == id);
        }

        // POST: api/Books/Find
        [Route("Find")]
        [HttpPost]
        public async Task<IActionResult> FindByName([FromBody] BookBinding BookBinding)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var Book = await _context.Books.SingleOrDefaultAsync(m => m.BookName == BookBinding.Name);

            _context.Entry(Book).Navigation("City").Load();

            if (Book == null)
            {
                return NotFound();
            }

            return Ok(Book);
        }


        // GET: api/Books
        [HttpGet]
        [Route("count")]
        public int GetCountBooks()
        {
            return _context.Books.Count();
        }
    }
}