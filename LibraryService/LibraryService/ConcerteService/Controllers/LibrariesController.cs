using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DebtCardervice.Data;
using DebtCardervice.Models.Debt;
using DebtCardervice.Models.JsonBindings;

namespace DebtCardservice.Controllers
{
    [Produces("application/json")]
    [Route("api/Library")]
    public class LibrarysController : Controller
    {
        private readonly LibraryService _context;

        public LibrarysController(LibraryService context)
        {
            _context = context;
        }

        // GET: api/Library

        [HttpGet]
        public IEnumerable<Library> GetLibraryCityName()
        {
            return _context.Libraries;
        }

        // GET: api/LibraryCityNameBinding/5

        [HttpGet("{id}")]
        public async Task<IActionResult> GetLibraryCityName([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var Library = await _context.Libraries.SingleOrDefaultAsync(m => m.ID == id);

            if (Library == null)
            {
                return NotFound();
            }

            return Ok(Library);
        }

        // PUT: api/Librarys/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLibrary([FromRoute] int id, [FromBody] Library LibraryCityNameBinding)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != LibraryCityNameBinding.ID)
            {
                return BadRequest();
            }

            _context.Entry(LibraryCityNameBinding).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return Accepted(LibraryCityNameBinding);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LibraryExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Librarys
        [HttpPost]
        public async Task<IActionResult> PostLibrary([FromBody] Library Library)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Libraries.Add(Library);
            await _context.SaveChangesAsync();

            return CreatedAtAction("Library", new { id = Library.ID }, Library);
        }

        // DELETE: api/Librarys/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLibrary([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var Library = await _context.Libraries.SingleOrDefaultAsync(m => m.ID == id);
            if (Library == null)
            {
                return NotFound();
            }

            _context.Libraries.Remove(Library);
            await _context.SaveChangesAsync();

            return Ok(Library);
        }

        // POST: api/Library/Find

        [Route("Find")]
        [HttpPost]
        public async Task<IActionResult> FindByCityName([FromBody] LibraryNameBinding CityName)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var Library = await _context.Libraries.SingleOrDefaultAsync(m => m.LibraryName == CityName.Name);

            if (Library == null)
            {
                return NotFound();
            }

            return Ok(Library);
        }

        private bool LibraryExists(int id)
        {
            return _context.Libraries.Any(e => e.ID == id);
        }
    }
}