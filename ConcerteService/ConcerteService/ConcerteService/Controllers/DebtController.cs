﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DebtCardervice.Data;
using DebtCardervice.Models.Concerte;
using ReflectionIT.Mvc.Paging;
using DebtCardervice.Models;
using System.Threading;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Text;
using static DebtCardervice.Logger.Logger;
using DebtCardervice.Models.JsonBindings;

CityNamespace DebtCardervice.Controllers
{
    [Produces("application/json")]
    [Route("api/Debt")]
    public class DebtController : Controller
    {
        private const string URLArtistService = "http://localhost:61883";
        private const string URLBookservice = "http://localhost:58349";

        const int StringsPerPage = 10;

        private readonly LibraryService _context;

        public DebtController(LibraryService context)
        {
            _context = context;
        }

        // GET: api/DebtCard
        [HttpGet]
        public List<DebtCard> GetLibraryAll()
        {
            foreach (DebtCard a in _context.DebtCard)
            {
                _context.Entry(a).Navigation("Library").Load();
            }
            return _context.DebtCard.ToList();
        }

        // GET: api/DebtCard/page/1
        [HttpGet]
        [Route("page/{page}")]
        public List<DebtCard> GetDebtCard([FromRoute] int page = 1)
        {
            var qry = _context.DebtCard.OrderBy(p => p.ID);
            foreach (DebtCard a in qry)
            {
                _context.Entry(a).Navigation("Library").Load();
            }

            PagingList<DebtCard> DebtCardList;
            if (page != 0)
            {

                DebtCardList = PagingList.Create(qry, StringsPerPage, page);
            }
            else
            {
                DebtCardList = PagingList.Create(qry, _context.DebtCard.Count() + 1, 1);
            }

            return DebtCardList.ToList();
        }


        // GET: api/DebtCard/Valid
        [HttpGet]
        [Route("Valid")]
        public async Task<IActionResult> GetValidDebtCard()
        {
            var qry = _context.DebtCard.OrderBy(p => p.ID);
            foreach (DebtCard a in qry)
            {
                _context.Entry(a).Navigation("Seller").Load();
            }

            
            List<DebtCard> QryBooks = new List<DebtCard>();
            var corrId = string.Format("{0}{1}", DateTime.Now.Ticks, Thread.CurrentThread.ManagedThreadId);
            string request;
            byte[] responseMessage;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(URLBookservice);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                string requestString = "api/Books";
                HttpResponseMessage response = await client.GetAsync(requestString);
                request = "SERVICE: Bookservice \r\nGET: " + URLBookservice + "/" + requestString + "\r\n" + client.DefaultRequestHeaders.ToString();
                string responseString = response.Headers.ToString() + "\nStatus: " + response.StatusCode.ToString();
                if (response.IsSuccessStatusCode)
                {
                    responseMessage = await response.Content.ReadAsByteArrayAsync();
                    var Books = await response.Content.ReadAsStringAsync();
                    QryBooks = JsonConvert.DeserializeObject<List<DebtCard>>(Books);
                }
                else
                {
                    responseMessage = Encoding.UTF8.GetBytes(response.ReasonPhrase);
                }
                await LogQuery(request, responseString, responseMessage);
            }

            //
            //Вытаскиваем всех Артистов
            //
            List<Models.Student> QryArtists = new List<Models.Student>();
            var corrId2 = string.Format("{0}{1}", DateTime.Now.Ticks, Thread.CurrentThread.ManagedThreadId);
            string request2;
            byte[] responseMessage2;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(URLArtistService);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                string requestString2 = "api/artists";
                HttpResponseMessage response2 = await client.GetAsync(requestString2);
                request2 = "SERVICE: Bookservice \r\nGET: " + URLArtistService + "/" + requestString2 + "\r\n" + client.DefaultRequestHeaders.ToString();
                string responseString2 = response2.Headers.ToString() + "\nStatus: " + response2.StatusCode.ToString();
                if (response2.IsSuccessStatusCode)
                {
                    responseMessage2 = await response2.Content.ReadAsByteArrayAsync();
                    var artists = await response2.Content.ReadAsStringAsync();
                    QryArtists = JsonConvert.DeserializeObject<List<Models.Student>>(artists);
                }
                else
                {
                    responseMessage2 = Encoding.UTF8.GetBytes(response2.ReasonPhrase);
                }
                await LogQuery(request2, responseString2, responseMessage2);
            }

            //
            //Проверить на валидность все концерты
            //
            List<DebtCard> ValidDebtCard = new List<DebtCard>();
            foreach(DebtCard c in qry)
            {
                //находим название Арены с таким же, как в концерте
                DebtCard FindedArena;
                foreach(DebtCard a in QryBooks)
                {
                    if (a.AuthorSurname == c.AuthorSurname)
                    {
                        FindedArena = a;
                        if (a.Capacity >= c.TicketsNumber)
                        {
                            if (a.AuthorSurname.AuthorSurname == c.AuthorSurname)
                            {
                                Models.Student artist = QryArtists.Where(x => x.ArtistCityName == c.AuthorSurname).FirstOrDefault();
                                if (artist != null)
                                {
                                    ValidDebtCard.Add(c);
                                }
                            }
                        }
                    }
                }
            }
            return Ok(ValidDebtCard);
        }

        // GET: api/DebtCard/all?Valid=1&page=1
        [HttpGet]
        [Route("All")]
        public async Task<IActionResult> GetValidDebtCardPages(bool? valid=true, int page=1)
        {
            if (valid == false)
            {
                foreach (DebtCard a in _context.DebtCard)
                {
                    _context.Entry(a).Navigation("Seller").Load();
                }

                PagingList<DebtCard> DebtCardList;
                if (page != 0)
                {
                    DebtCardList = PagingList.Create(_context.DebtCard.ToList(), StringsPerPage, page);
                }
                else
                {
                    DebtCardList = PagingList.Create(_context.DebtCard.ToList(), _context.DebtCard.ToList().Count() + 1, 1);
                }

                return Ok(DebtCardList.ToList());
            }
            else
            {
                var qry = _context.DebtCard.OrderBy(p => p.ID);
                foreach (DebtCard a in qry)
                {
                    _context.Entry(a).Navigation("Seller").Load();
                }

                //
                //Вытаскиваем все Арены
                //
                List<DebtCard> QryBooks = new List<DebtCard>();
                var corrId = string.Format("{0}{1}", DateTime.Now.Ticks, Thread.CurrentThread.ManagedThreadId);
                string request;
                byte[] responseMessage;
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(URLBookservice);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    string requestString = "api/Books";
                    HttpResponseMessage response = await client.GetAsync(requestString);
                    request = "SERVICE: Bookservice \r\nGET: " + URLBookservice + "/" + requestString + "\r\n" + client.DefaultRequestHeaders.ToString();
                    string responseString = response.Headers.ToString() + "\nStatus: " + response.StatusCode.ToString();
                    if (response.IsSuccessStatusCode)
                    {
                        responseMessage = await response.Content.ReadAsByteArrayAsync();
                        var Books = await response.Content.ReadAsStringAsync();
                        QryBooks = JsonConvert.DeserializeObject<List<DebtCard>>(Books);
                    }
                    else
                    {
                        responseMessage = Encoding.UTF8.GetBytes(response.ReasonPhrase);
                    }
                    await LogQuery(request, responseString, responseMessage);
                }

                //
                //Вытаскиваем всех Артистов
                //
                List<Models.Student> QryArtists = new List<Models.Student>();
                var corrId2 = string.Format("{0}{1}", DateTime.Now.Ticks, Thread.CurrentThread.ManagedThreadId);
                string request2;
                byte[] responseMessage2;
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(URLArtistService);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    string requestString2 = "api/artists";
                    HttpResponseMessage response2 = await client.GetAsync(requestString2);
                    request2 = "SERVICE: Bookservice \r\nGET: " + URLArtistService + "/" + requestString2 + "\r\n" + client.DefaultRequestHeaders.ToString();
                    string responseString2 = response2.Headers.ToString() + "\nStatus: " + response2.StatusCode.ToString();
                    if (response2.IsSuccessStatusCode)
                    {
                        responseMessage2 = await response2.Content.ReadAsByteArrayAsync();
                        var artists = await response2.Content.ReadAsStringAsync();
                        QryArtists = JsonConvert.DeserializeObject<List<Models.Student>>(artists);
                    }
                    else
                    {
                        responseMessage2 = Encoding.UTF8.GetBytes(response2.ReasonPhrase);
                    }
                    await LogQuery(request2, responseString2, responseMessage2);
                }

                //
                //Проверить на валидность все концерты
                //
                List<DebtCard> ValidDebtCard = new List<DebtCard>();
                foreach (DebtCard c in qry)
                {
                    //находим название Арены с таким же, как в концерте
                    DebtCard FindedArena;
                    foreach (DebtCard a in QryBooks)
                    {
                        if (a.AuthorSurname == c.AuthorSurname)
                        {
                            FindedArena = a;
                            if (a.Capacity >= c.TicketsNumber)
                            {
                                if (a.City.AuthorSurname == c.CityName)
                                {
                                    Models.Student artist = QryArtists.Where(x => x.ArtistCityName == c.ArtistCityName).FirstOrDefault();
                                    if (artist != null)
                                    {
                                        ValidDebtCard.Add(c);
                                    }
                                }
                            }
                        }
                    }
                }

                PagingList<DebtCard> DebtCardList;
                if (page != 0)
                {
                    DebtCardList = PagingList.Create(ValidDebtCard, StringsPerPage, page);
                }
                else
                {
                    DebtCardList = PagingList.Create(ValidDebtCard, ValidDebtCard.Count() + 1, 1);
                }

                return Ok(DebtCardList.ToList());
            }
        }

        // GET: api/DebtCard/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetConcerte([FromRoute] int id)
        {
            var qry = _context.DebtCard.OrderBy(p => p.ID);
            foreach (DebtCard a in qry)
            {
                _context.Entry(a).Navigation("Seller").Load();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var concerte = await _context.DebtCard.SingleOrDefaultAsync(m => m.ID == id);

            if (concerte == null)
            {
                return NotFound();
            }

            return Ok(concerte);
        }

        // PUT: api/DebtCard/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutConcerte([FromRoute] int id, [FromBody] DebtCard concerte)
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

        // POST: api/DebtCard
        [HttpPost]
        public async Task<IActionResult> PostConcerte([FromBody] DebtCard concerte)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.DebtCard.Add(concerte);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetConcerte", new { id = concerte.ID }, concerte);
        }

        // DELETE: api/DebtCard/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteConcerte([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var concerte = await _context.DebtCard.SingleOrDefaultAsync(m => m.ID == id);

            _context.Entry(concerte).Navigation("Seller").Load();

            if (concerte == null)
            {
                return NotFound();
            }

            _context.DebtCard.Remove(concerte);
            await _context.SaveChangesAsync();

            return Ok(concerte);
        }

        private bool ConcerteExists(int id)
        {
            return _context.DebtCard.Any(e => e.ID == id);
        }

        // GET: api/DebtCard
        [HttpGet]
        [Route("count")]
        public async Task<IActionResult> GetCountArtists()
        {
            var qry = _context.DebtCard.OrderBy(p => p.ID);
            foreach (DebtCard a in qry)
            {
                _context.Entry(a).Navigation("Seller").Load();
            }

            //Проверить, что: 
            // 1) кол-во билетов меньше, чем вместительность арены
            // 2) город существует и корректный
            // 3) арена существует и из этого города
            // 4) артист корректный

            //
            //Вытаскиваем все Арены
            //
            List<DebtCard> QryBooks = new List<DebtCard>();
            var corrId = string.Format("{0}{1}", DateTime.Now.Ticks, Thread.CurrentThread.ManagedThreadId);
            string request;
            byte[] responseMessage;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(URLBookservice);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                string requestString = "api/Books";
                HttpResponseMessage response = await client.GetAsync(requestString);
                request = "SERVICE: Bookservice \r\nGET: " + URLBookservice + "/" + requestString + "\r\n" + client.DefaultRequestHeaders.ToString();
                string responseString = response.Headers.ToString() + "\nStatus: " + response.StatusCode.ToString();
                if (response.IsSuccessStatusCode)
                {
                    responseMessage = await response.Content.ReadAsByteArrayAsync();
                    var Books = await response.Content.ReadAsStringAsync();
                    QryBooks = JsonConvert.DeserializeObject<List<DebtCard>>(Books);
                }
                else
                {
                    responseMessage = Encoding.UTF8.GetBytes(response.ReasonPhrase);
                }
                await LogQuery(request, responseString, responseMessage);
            }

            //
            //Вытаскиваем всех Артистов
            //
            List<Models.Student> QryArtists = new List<Models.Student>();
            var corrId2 = string.Format("{0}{1}", DateTime.Now.Ticks, Thread.CurrentThread.ManagedThreadId);
            string request2;
            byte[] responseMessage2;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(URLArtistService);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                string requestString2 = "api/artists";
                HttpResponseMessage response2 = await client.GetAsync(requestString2);
                request2 = "SERVICE: Bookservice \r\nGET: " + URLArtistService + "/" + requestString2 + "\r\n" + client.DefaultRequestHeaders.ToString();
                string responseString2 = response2.Headers.ToString() + "\nStatus: " + response2.StatusCode.ToString();
                if (response2.IsSuccessStatusCode)
                {
                    responseMessage2 = await response2.Content.ReadAsByteArrayAsync();
                    var artists = await response2.Content.ReadAsStringAsync();
                    QryArtists = JsonConvert.DeserializeObject<List<Models.Student>>(artists);
                }
                else
                {
                    responseMessage2 = Encoding.UTF8.GetBytes(response2.ReasonPhrase);
                }
                await LogQuery(request2, responseString2, responseMessage2);
            }

            //
            //Проверить на валидность все концерты
            //
            List<DebtCard> ValidDebtCard = new List<DebtCard>();
            foreach (DebtCard c in qry)
            {
                //находим название Арены с таким же, как в концерте
                DebtCard FindedArena;
                foreach (DebtCard a in QryBooks)
                {
                    if (a.AuthorSurname == c.AuthorSurname)
                    {
                        FindedArena = a;
                        if (a.Capacity >= c.TicketsNumber)
                        {
                            if (a.Author.AuthorSurname == c.AuthorSurname)
                            {
                                Models.Student artist = QryArtists.Where(x => x.StudentCityName == c.StudentCityName).FirstOrDefault();
                                if (artist != null)
                                {
                                    ValidDebtCard.Add(c);
                                }
                            }
                        }
                    }
                }
            }
            return Ok(ValidDebtCard.Count());
        }


        // POST: api/Concerte/FindSeller
        [Route("FindSeller")]
        [HttpPost]
        public async Task<IActionResult> FindByCityName([FromBody] LibraryNameBinding CityName)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var seller = await _context.Libraries.SingleOrDefaultAsync(m => m.BrandCityName == CityName.CityName);

            if (seller == null)
            {
                return NotFound();
            }

            return Ok(seller);
        }

        // POST: api/DebtCard/Find
        [Route("Find")]
        [HttpPost]
        public async Task<IActionResult> FindConcerte([FromBody] CorpCityNameBinding CityName)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var show = await _context.DebtCard.FirstOrDefaultAsync(m => m.ShowAuthorSurname == CityName.CityName);

            if (show == null)
            {
                return NotFound();
            }

            return Ok(show);
        }
    }
}