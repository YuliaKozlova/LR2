using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ConcerteService.Data;
using ConcerteService.Models;
using ReflectionIT.Mvc.Paging;
using StudentService.Models.JsonBindings;

namespace ConcerteService.Controllers
{
    [Produces("application/json")]
    [Route("api/Students")]
    public class StudentsController : Controller
    {
        private readonly StudentContext _context;

        const int StringsPerPage = 10;

        public StudentsController(StudentContext context)
        {
            _context = context;
        }

        // GET: api/Students
        [HttpGet]
        public IEnumerable<Student> GetStudents()
        {
            return _context.Students;
        }

        // GET: api/Students/page/{id}
        [HttpGet]
        [Route("page/{page}")]
        public List<Student> GetStudents([FromRoute] int page = 1)
        {
            var qry = _context.Students.OrderBy(p => p.StudentName);

            PagingList<Student> StudentList;
            if (page != 0)
            {
                StudentList = PagingList.Create(qry, StringsPerPage, page);
            }
            else
            {
                StudentList = PagingList.Create(qry, _context.Students.Count() + 1, 1);
            }

            return StudentList.ToList();
        }


        // GET: api/Students/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetStudent([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var Student = await _context.Students.SingleOrDefaultAsync(m => m.ID == id);

            if (Student == null)
            {
                return NotFound();
            }

            return Ok(Student);
        }

        // POST: api/Students/Find
        [Route("Find")]
        [HttpPost]
        public async Task<IActionResult> FindByName([FromBody] StudentNameBinding name)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var Student = await _context.Students.SingleOrDefaultAsync(m => m.StudentName == name.Name);

            if (Student == null)
            {
                return NotFound();
            }

            return Ok(Student);
        }

        // PUT: api/Students/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStudent([FromRoute] int id, [FromBody] Student Student)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != Student.ID)
            {
                return BadRequest();
            }

            _context.Entry(Student).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return Accepted(Student);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudentExists(id))
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

        // POST: api/Students
        [HttpPost]
        public async Task<IActionResult> PostStudent([FromBody] Student Student)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Students.Add(Student);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetStudent", new { id = Student.ID }, Student);
        }

        // DELETE: api/Students/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var Student = await _context.Students.SingleOrDefaultAsync(m => m.ID == id);
            if (Student == null)
            {
                return NotFound();
            }

            _context.Students.Remove(Student);
            await _context.SaveChangesAsync();

            return Ok(Student);
        }

        private bool StudentExists(int id)
        {
            return _context.Students.Any(e => e.ID == id);
        }

        // GET: api/Student
        [HttpGet]
        [Route("count")]
        public int GetCountStudents()
        {
            return _context.Students.Count();
        }
    }
}