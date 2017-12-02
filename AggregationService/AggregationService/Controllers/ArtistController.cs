using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AggregationService.Models.ArtistService;
using System.Threading;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Text;
using static AggregationService.Logger.Logger;
using AggregationService.Models.ModelsForView;
using Newtonsoft.Json.Linq;

namespace AggregationService.Controllers
{
    [Route("Artist")]
    public class ArtistController : Controller
    {
        private const string URLArtistService = "http://localhost:61883";


        // GET: Artist
        [HttpGet("{id?}")]
        public async Task<IActionResult> Index([FromRoute] int id = 1)
        {
            List<Artist> result = new List<Artist>();
            int count = 0;

            /**/
            var corrId = string.Format("{0}{1}", DateTime.Now.Ticks, Thread.CurrentThread.ManagedThreadId);
            /**/
            string request;
            //byte[] requestMessage;
            /**/
            byte[] responseMessage;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(URLArtistService);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                /**/
                string requestString = "api/artists/page/" + id;
                HttpResponseMessage response = await client.GetAsync(requestString);

                /**/
                request = "SERVICE: ArtistService \r\nGET: " + URLArtistService + "/" + requestString + "\r\n" + client.DefaultRequestHeaders.ToString();
                /**/
                string responseString = response.Headers.ToString() + "\nStatus: " + response.StatusCode.ToString();

                if (response.IsSuccessStatusCode)
                {
                    /**/
                    responseMessage = await response.Content.ReadAsByteArrayAsync();
                    var artists = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<List<Artist>>(artists);
                }
                else
                {
                    /**/
                    responseMessage = Encoding.UTF8.GetBytes(response.ReasonPhrase);
                    return Error();
                }

                /**/
                await LogQuery(request, responseString, responseMessage);


                //
                // ПОЛУЧАЕМ КОЛ-ВО СУЩНОСТЕЙ В БД МИКРОСЕРВИСА, ЧТОБЫ УЗНАТЬ, СКОЛЬКО СТРАНИЦ РИСОВАТЬ
                //
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                string requestStringCount = "api/artists/count";
                HttpResponseMessage responseStringsCount = await client.GetAsync(requestStringCount);

                /**/
                request = "SERVICE: ArtistService \r\nGET: " + URLArtistService + "/" + requestString + "\r\n" + client.DefaultRequestHeaders.ToString();
                /**/
                responseString = responseStringsCount.Headers.ToString() + "\nStatus: " + responseStringsCount.StatusCode.ToString();

                if (responseStringsCount.IsSuccessStatusCode)
                {
                    /**/
                    responseMessage = await responseStringsCount.Content.ReadAsByteArrayAsync();
                    var countStringsContent = await responseStringsCount.Content.ReadAsStringAsync();
                    count = JsonConvert.DeserializeObject<int>(countStringsContent);
                }
                else
                {
                    /**/
                    responseMessage = Encoding.UTF8.GetBytes(responseStringsCount.ReasonPhrase);
                    return Error();
                }
                ArtistList resultQuery = new ArtistList() { artists = result, countArtists = count };

                /**/
                await LogQuery(request, responseString, responseMessage);

                return View(resultQuery);
            }
        }


        [Route("AddArtist")]
        public IActionResult AddArtist()
        {
            return View();
        }


        [Route("AddArtist")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddArtist([Bind("ArtistName,LastFmRating")] Artist artist)
        {
            //СЕРИАЛИЗУЕМ artist и посылаем на ArtistService
            var values = new JObject();
            values.Add("ArtistName", artist.ArtistName);
            values.Add("LastFmRating", artist.LastFmRating);

            /**/
            var corrId = string.Format("{0}{1}", DateTime.Now.Ticks, Thread.CurrentThread.ManagedThreadId);
            /**/
            string request;
            /**/
            string requestMessage = values.ToString();
            /**/
            byte[] responseMessage;

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(URLArtistService);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpContent content = new StringContent(values.ToString(), Encoding.UTF8, "application/json");

            /**/
            string requestString = "api/Artists";

            var response = await client.PostAsJsonAsync(requestString, values);

            if ((int)response.StatusCode == 500)
            {
                string description = "There is no city with ID (" + artist.ID + ")";
                ResponseMessage message = new ResponseMessage();
                message.description = description;
                message.message = response;
                return View("Error", message);
            }

            /**/
            request = "SERVICE: ArtistService \r\nPOST: " + URLArtistService + "/" + requestString + "\r\n" + client.DefaultRequestHeaders.ToString();
            /**/
            string responseString = response.Headers.ToString() + "\nStatus: " + response.StatusCode.ToString();

            if (response.IsSuccessStatusCode)
            {
                /**/
                responseMessage = await response.Content.ReadAsByteArrayAsync();
                /**/
                await LogQuery(request, requestMessage, responseString, responseMessage);
                return RedirectToAction(nameof(Index));
            }
            else
            {
                /**/
                responseMessage = Encoding.UTF8.GetBytes(response.ReasonPhrase);
                /**/
                await LogQuery(request, requestMessage, responseString, responseMessage);
                string description = "Another error ";
                ResponseMessage message = new ResponseMessage();
                message.description = description;
                message.message = response;
                //return View(message);
                return View("Error", message);
            }
        }

        [Route("Error")]
        public IActionResult Error()
        {
            return View("Error");
        }


        [HttpGet("Delete/{id?}")]
        //[HttpDelete("{id?}")]
        public async Task<IActionResult> Delete(int id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(URLArtistService);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                /**/
                var corrId = string.Format("{0}{1}", DateTime.Now.Ticks, Thread.CurrentThread.ManagedThreadId);
                /**/
                string request;
                //byte[] requestMessage;
                /**/
                byte[] responseMessage;

                string route = "api/artists/" + id;

                /**/
                string requestString = route;

                HttpResponseMessage response = await client.DeleteAsync(route);

                /**/
                request = "SERVICE: ArtistService \r\nDELETE: " + URLArtistService + "/" + requestString + "\r\n" + client.DefaultRequestHeaders.ToString();
                /**/
                string responseString = response.Headers.ToString() + "\nStatus: " + response.StatusCode.ToString();

                if (response.IsSuccessStatusCode)
                {
                    /**/
                    responseMessage = await response.Content.ReadAsByteArrayAsync();
                    /**/
                    await LogQuery(request, responseString, responseMessage);
                    return RedirectToAction(nameof(Index), new { id = 1 });
                }
                else
                {
                    /**/
                    responseMessage = Encoding.UTF8.GetBytes(response.ReasonPhrase);
                    /**/
                    await LogQuery(request, responseString, responseMessage);
                    return View("Error");
                }
            }
        }


        [HttpGet("Edite/{id?}")]
        public async Task<IActionResult> Edite(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }


            //
            // ПОЛУЧАЕМ СУЩНОСТЬ с ID
            //
            Artist artist;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(URLArtistService);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                string requestString = "api/Artists/" + id;
                HttpResponseMessage response = await client.GetAsync(requestString);

                /**/
                string request = "SERVICE: ArtistService \r\nGET: " + URLArtistService + "/" + "\r\n" + client.DefaultRequestHeaders.ToString();
                /**/
                string responseString = response.Headers.ToString() + "\nStatus: " + response.StatusCode.ToString();
                /**/
                byte[] responseMessage;

                if (response.IsSuccessStatusCode)
                {
                    /**/
                    responseMessage = await response.Content.ReadAsByteArrayAsync();
                    var artistContent = await response.Content.ReadAsStringAsync();
                    artist = JsonConvert.DeserializeObject<Artist>(artistContent);
                    if (artist == null)
                    {
                        /**/
                        await LogQuery(request, responseString, responseMessage);
                        return NotFound();
                    }
                    /**/
                    await LogQuery(request, responseString, responseMessage);
                    return View(artist);
                }
                else
                {
                    /**/
                    responseMessage = Encoding.UTF8.GetBytes(response.ReasonPhrase);
                    /**/
                    await LogQuery(request, responseString, responseMessage);
                    return Error();
                }
            }
        }


        [Route("Edite/{id}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edite([Bind("ID,ArtistName,LastFmRating")] Artist artist)
        {
            if (ModelState.IsValid)
            {
                //СЕРИАЛИЗУЕМ artist и посылаем на ArtistService
                var values = new JObject();
                values.Add("ID", artist.ID);
                values.Add("ArtistName", artist.ArtistName);
                values.Add("LastFmRating", artist.LastFmRating);

                /**/
                var corrId = string.Format("{0}{1}", DateTime.Now.Ticks, Thread.CurrentThread.ManagedThreadId);
                /**/
                string request;
                /**/
                string requestMessage = values.ToString();
                /**/
                byte[] responseMessage;

                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(URLArtistService);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpContent content = new StringContent(values.ToString(), Encoding.UTF8, "application/json");

                /**/
                string requestString = "api/artists/" + artist.ID;

                var response = await client.PutAsJsonAsync(requestString, values);

                /**/
                request = "SERVICE: ArtistService \r\nPUT: " + URLArtistService + "/" + requestString + "\r\n" + client.DefaultRequestHeaders.ToString();
                /**/
                string responseString = response.Headers.ToString() + "\nStatus: " + response.StatusCode.ToString();

                if ((int)response.StatusCode == 500)
                {
                    string description = "There is no artist with ID (" + artist.ID + ")";
                    ResponseMessage message = new ResponseMessage();
                    message.description = description;
                    message.message = response;
                    return View("Error", message);
                }

                if (response.IsSuccessStatusCode)
                {
                    /**/
                    responseMessage = await response.Content.ReadAsByteArrayAsync();
                    /**/
                    await LogQuery(request, requestMessage, responseString, responseMessage);
                    return RedirectToAction(nameof(Index), new { id = 1 });
                }
                else
                {
                    /**/
                    responseMessage = Encoding.UTF8.GetBytes(response.ReasonPhrase);
                    /**/
                    await LogQuery(request, requestMessage, responseString, responseMessage);
                    return View(response);
                }
            }
            else
            {
                return View();
            }
        }
    }
}