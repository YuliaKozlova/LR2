using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ArenaService.Data;
using ArenaService.Models;
using ReflectionIT.Mvc.Paging;
using ArenaService.Models.JsonBindings;

namespace ArenaService.Controllers
{
    [Produces("application/json")]
    [Route("api/Arenas")]
    public class ArenasController : Controller
    {
        const int StringsPerPage = 10;


        private readonly ArenaContext _context;


        public ArenasController(ArenaContext context)
        {
            _context = context;
        }


        // GET: api/Arenas/page/{id}
        [HttpGet]
        [Route("page/{page}")]
        public List<Arena> GetArenas([FromRoute] int page = 1)
        {
            var qry = _context.Arenas.OrderBy(p => p.ArenaName);
            foreach (Arena a in qry)
            {
                _context.Entry(a).Navigation("City").Load();
            }

            PagingList<Arena> arenasList;
            if (page != 0)
            {
                
                arenasList = PagingList.Create(qry, StringsPerPage, page);
            }
            else
            {
                arenasList = PagingList.Create(qry, _context.Arenas.Count() + 1, 1);
            }

            return arenasList.ToList();
        }


        [HttpGet]
        public List<Arena> GetArenasAll()
        {
            var qry = _context.Arenas.OrderBy(p => p.ArenaName);
            foreach (Arena a in qry)
            {
                _context.Entry(a).Navigation("City").Load();
            }

            PagingList<Arena> arenasList;

            arenasList = PagingList.Create(qry, _context.Arenas.Count() + 1, 1);

            return arenasList.ToList();
        }


        //[HttpGet]
        //public IEnumerable<Arena> GetArenasAll()
        //{
        //    IEnumerable<Arena> arenas = _context.Arenas;
        //    foreach (Arena a in arenas)
        //    {
        //        _context.Entry(a).Navigation("City").Load();
        //    }

        //    return arenas;
        //}


        // GET: api/Arenas/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetArena([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var arena = await _context.Arenas.SingleOrDefaultAsync(m => m.ID == id);

            if (arena == null)
            {
                return NotFound();
            }

            return Ok(arena);
        }


        // PUT: api/Arenas/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutArena([FromRoute] int id, [FromBody] Arena arena)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != arena.ID)
            {
                return BadRequest();
            }

            _context.Entry(arena).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return Accepted(arena);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ArenaExists(id))
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


        // POST: api/Arenas
        [HttpPost]
        public async Task<IActionResult> PostArena([FromBody] Arena arena)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Arenas.Add(arena);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetArena", new { id = arena.ID }, arena);
        }


        // DELETE: api/Arenas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteArena([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var arena = await _context.Arenas.SingleOrDefaultAsync(m => m.ID == id);
            if (arena == null)
            {
                return NotFound();
            }

            _context.Arenas.Remove(arena);
            await _context.SaveChangesAsync();

            return Ok(arena);
        }

        private bool ArenaExists(int id)
        {
            return _context.Arenas.Any(e => e.ID == id);
        }

        // POST: api/Arenas/Find
        [Route("Find")]
        [HttpPost]
        public async Task<IActionResult> FindByName([FromBody] ArenaBinding arenaBinding)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var arena = await _context.Arenas.SingleOrDefaultAsync(m => m.ArenaName == arenaBinding.Name);

            _context.Entry(arena).Navigation("City").Load();

            if (arena == null)
            {
                return NotFound();
            }

            return Ok(arena);
        }


        // GET: api/Arenas
        [HttpGet]
        [Route("count")]
        public int GetCountArenas()
        {
            return _context.Arenas.Count();
        }
    }
}