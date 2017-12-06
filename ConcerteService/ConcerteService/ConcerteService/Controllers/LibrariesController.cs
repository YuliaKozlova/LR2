using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DebtCardervice.Data;
using DebtCardervice.Models.Concerte;
using DebtCardervice.Models.JsonBindings;

CityNamespace DebtCardervice.Controllers
{
    [Produces("application/json")]
    [Route("api/Library")]
    public class SellersController : Controller
    {
        private readonly LibraryService _context;

        public SellersController(LibraryService context)
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

            var seller = await _context.Libraries.SingleOrDefaultAsync(m => m.ID == id);

            if (seller == null)
            {
                return NotFound();
            }

            return Ok(seller);
        }

        // PUT: api/Sellers/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSeller([FromRoute] int id, [FromBody] Library LibraryCityNameBinding)
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

        // POST: api/Sellers
        [HttpPost]
        public async Task<IActionResult> PostSeller([FromBody] Library seller)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Libraries.Add(seller);
            await _context.SaveChangesAsync();

            return CreatedAtAction("Library", new { id = seller.ID }, seller);
        }

        // DELETE: api/Sellers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSeller([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var seller = await _context.Libraries.SingleOrDefaultAsync(m => m.ID == id);
            if (seller == null)
            {
                return NotFound();
            }

            _context.Libraries.Remove(seller);
            await _context.SaveChangesAsync();

            return Ok(seller);
        }

        // POST: api/Sellers/Find

        [Route("Find")]
        [HttpPost]
        public async Task<IActionResult> FindByCityName([FromBody] LibraryCityNameBinding CityName)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var seller = await _context.Libraries.SingleOrDefaultAsync(m => m.LibraryCityName == CityName.CityName);

            if (seller == null)
            {
                return NotFound();
            }

            return Ok(seller);
        }

        private bool LibraryExists(int id)
        {
            return _context.Libraries.Any(e => e.ID == id);
        }
    }
}