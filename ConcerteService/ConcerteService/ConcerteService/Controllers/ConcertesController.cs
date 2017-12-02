using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ConcerteService.Data;
using ConcerteService.Models.Concerte;
using ReflectionIT.Mvc.Paging;

namespace ConcerteService.Controllers
{
    [Produces("application/json")]
    [Route("api/Concertes")]
    public class ConcertesController : Controller
    {
        private const string URLArtistService = "http://localhost:61883";
        private const string URLArenaService = "http://localhost:58349";

        const int StringsPerPage = 10;

        private readonly ConcerteContext _context;

        public ConcertesController(ConcerteContext context)
        {
            _context = context;
        }

        // GET: api/Concertes
        [HttpGet]
        public List<Concerte> GetConcertesAll()
        {
            foreach (Concerte a in _context.Concerts)
            {
                _context.Entry(a).Navigation("Seller").Load();
            }
            return _context.Concerts.ToList();
        }

        // GET: api/Concertes/page/1
        [HttpGet]
        [Route("page/{page}")]
        public List<Concerte> GetConcertes([FromRoute] int page = 1)
        {
            var qry = _context.Concerts.OrderBy(p => p.ID);
            foreach (Concerte a in qry)
            {
                _context.Entry(a).Navigation("Seller").Load();
            }

            PagingList<Concerte> concertesList;
            if (page != 0)
            {

                concertesList = PagingList.Create(qry, StringsPerPage, page);
            }
            else
            {
                concertesList = PagingList.Create(qry, _context.Concerts.Count() + 1, 1);
            }

            return concertesList.ToList();
        }


        // GET: api/Concertes/Valid
        [HttpGet]
        [Route("Valid")]
        public List<Concerte> GetValidConcertes([FromRoute] int page = 1)
        {
            var qry = _context.Concerts.OrderBy(p => p.ID);
            foreach (Concerte a in qry)
            {
                _context.Entry(a).Navigation("Seller").Load();
            }

            //Проверить, что: 
            // 1) кол-во билетов меньше, чем вместительность арены
            // 2) город существует и корректный
            // 3) арена существует и из этого города
            // 4) артист корректный



            // ДЛЯ КАЖДОЙ СУЩНОСТИ КОНЦЕРТА: 
            // 1. подаем название арены в ArenaService в json: {"Name": "crocus city Hall"}.
            // 2. Сравниваем город с городом арены
            // 3. сравниваем кол-во билетов с вместительностью арены
            // 4. подаем название артиста в http://localhost:61883/api/Artists/Find  { "Name": "Linkin Park" }.

            foreach (Concerte a in qry)
            {
                string ArenaName = a.ArenaName;
            }



           
        }

        // GET: api/Concertes/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetConcerte([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var concerte = await _context.Concerts.SingleOrDefaultAsync(m => m.ID == id);

            if (concerte == null)
            {
                return NotFound();
            }

            return Ok(concerte);
        }

        // PUT: api/Concertes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutConcerte([FromRoute] int id, [FromBody] Concerte concerte)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != concerte.ID)
            {
                return BadRequest();
            }

            _context.Entry(concerte).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return Accepted(concerte);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ConcerteExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }

        // POST: api/Concertes
        [HttpPost]
        public async Task<IActionResult> PostConcerte([FromBody] Concerte concerte)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Concerts.Add(concerte);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetConcerte", new { id = concerte.ID }, concerte);
        }

        // DELETE: api/Concertes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteConcerte([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var concerte = await _context.Concerts.SingleOrDefaultAsync(m => m.ID == id);
            if (concerte == null)
            {
                return NotFound();
            }

            _context.Concerts.Remove(concerte);
            await _context.SaveChangesAsync();

            return Ok(concerte);
        }

        private bool ConcerteExists(int id)
        {
            return _context.Concerts.Any(e => e.ID == id);
        }
    }
}