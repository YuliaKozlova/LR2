using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AggregationService.Models;
using System.Net;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using AggregationService.Models.ArenaService;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;
using Newtonsoft;
using System.Net.Http.Formatting;
using System.Data.Entity;
using AggregationService.Models.ModelsForView;
using System.Threading;
using static AggregationService.Logger.Logger;

namespace AggregationService.Controllers
{
    [Route("Arena")]
    public class ArenaController : Controller
    {
        private const string URLArenaService = "http://localhost:58349";


        //[HttpGet("Index/{id?}")]
        [HttpGet("{id?}")]
        public async Task<IActionResult> Index([FromRoute] int id = 1)
        {
            List<Arena> result = new List<Arena>();
            int count = 0;

            /**/var corrId = string.Format("{0}{1}", DateTime.Now.Ticks, Thread.CurrentThread.ManagedThreadId);         
            /**/string request;
            //byte[] requestMessage;
            /**/byte[] responseMessage;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(URLArenaService);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                /**/string requestString = "api/arenas/page/" + id;
                HttpResponseMessage response = await client.GetAsync(requestString);
                
                /**/request = "SERVICE: ArenaService \r\nGET: " + URLArenaService + "/" + requestString + "\r\n" + client.DefaultRequestHeaders.ToString();
                /**/string responseString = response.Headers.ToString() + "\nStatus: " + response.StatusCode.ToString();

                if (response.IsSuccessStatusCode)
                {
                    /**/responseMessage = await response.Content.ReadAsByteArrayAsync();
                    var arenas = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<List<Arena>>(arenas);
                }
                else
                {
                    /**/responseMessage = Encoding.UTF8.GetBytes(response.ReasonPhrase);
                    return Error();
                }

                /**/await LogQuery(request, responseString, responseMessage);


                //
                // ПОЛУЧАЕМ КОЛ-ВО СУЩНОСТЕЙ В БД МИКРОСЕРВИСА, ЧТОБЫ УЗНАТЬ, СКОЛЬКО СТРАНИЦ РИСОВАТЬ
                //
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                string requestStringCount = "api/arenas/count";
                HttpResponseMessage responseStringsCount = await client.GetAsync(requestStringCount);

                /**/request = "SERVICE: ArenaService \r\nGET: " + URLArenaService + "/" + requestString + "\r\n" + client.DefaultRequestHeaders.ToString();
                /**/responseString = responseStringsCount.Headers.ToString() + "\nStatus: " + responseStringsCount.StatusCode.ToString();

                if (responseStringsCount.IsSuccessStatusCode)
                {
                    /**/responseMessage = await responseStringsCount.Content.ReadAsByteArrayAsync();
                    var countStringsContent = await responseStringsCount.Content.ReadAsStringAsync();
                    count = JsonConvert.DeserializeObject<int>(countStringsContent);
                }
                else
                {
                    /**/responseMessage = Encoding.UTF8.GetBytes(responseStringsCount.ReasonPhrase);
                    return Error();
                }
                ArenaList resultQuery = new ArenaList() { arenas = result, countArenas = count };

                /**/await LogQuery(request, responseString, responseMessage);

                return View(resultQuery);
            }
        }

        //[HttpGet("{page?}")]
        //public IActionResult IndexHead([FromRoute] int page = 1)
        //{
        //    return RedirectToAction(nameof(Index), new { id = page });
        //}

        [Obsolete]
        [Route("AddArena")]
        public IActionResult AddArena()
        {
            return View();
        }


        [Route("AddArena")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddArena([Bind("ArenaName,Capacity,CityID")] Arena arena)
        {
            //СЕРИАЛИЗУЕМ arena и посылаем на ArenaService
            var values = new JObject();
            values.Add("ArenaName", arena.ArenaName);
            values.Add("CityID", arena.CityID);
            values.Add("Capacity", arena.Capacity);

            /**/
            var corrId = string.Format("{0}{1}", DateTime.Now.Ticks, Thread.CurrentThread.ManagedThreadId);
            /**/string request;
            /**/string requestMessage = values.ToString();
            /**/byte[] responseMessage;

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(URLArenaService);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpContent content = new StringContent(values.ToString(), Encoding.UTF8, "application/json");

            /**/string requestString = "api/arenas";

            var response = await client.PostAsJsonAsync("api/arenas", values);

            if ((int)response.StatusCode == 500)
            {
                string description = "There is no city with ID (" + arena.CityID + ")";
                ResponseMessage message = new ResponseMessage();
                message.description = description;
                message.message = response;
                return View("Error", message);
            }

            /**/request = "SERVICE: ArenaService \r\nPOST: " + URLArenaService + "/" + requestString + "\r\n" + client.DefaultRequestHeaders.ToString();
            /**/string responseString = response.Headers.ToString() + "\nStatus: " + response.StatusCode.ToString();

            if (response.IsSuccessStatusCode)
            {
                /**/responseMessage = await response.Content.ReadAsByteArrayAsync();
                /**/await LogQuery(request, requestMessage, responseString, responseMessage);
                return RedirectToAction(nameof(Index));
            }
            else
            {
                /**/responseMessage = Encoding.UTF8.GetBytes(response.ReasonPhrase);
                /**/await LogQuery(request, requestMessage, responseString, responseMessage);
                string description = "Another error ";
                ResponseMessage message = new ResponseMessage();
                message.description = description;
                message.message = response;
                //return View(message);
                return View("Error", message);
            } 
        }


        [HttpGet("Delete/{id?}")]
        public async Task<IActionResult> Delete(int id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(URLArenaService);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                /**/var corrId = string.Format("{0}{1}", DateTime.Now.Ticks, Thread.CurrentThread.ManagedThreadId);
                /**/string request;
                //byte[] requestMessage;
                /**/byte[] responseMessage;

                string route = "api/arenas/" + id;

                /**/string requestString = route;

                HttpResponseMessage response = await client.DeleteAsync(route);

                /**/request = "SERVICE: ArenaService \r\nDELETE: " + URLArenaService + "/" + requestString + "\r\n" + client.DefaultRequestHeaders.ToString();
                /**/string responseString = response.Headers.ToString() + "\nStatus: " + response.StatusCode.ToString();

                if (response.IsSuccessStatusCode)
                {
                    /**/responseMessage = await response.Content.ReadAsByteArrayAsync();
                    /**/await LogQuery(request, responseString, responseMessage);
                    return RedirectToAction(nameof(Index), new { id = 1 });
                }
                else
                {
                    /**/responseMessage = Encoding.UTF8.GetBytes(response.ReasonPhrase);
                    /**/await LogQuery(request, responseString, responseMessage);
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
            Arena arena;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(URLArenaService);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                string requestString = "api/arenas/" + id;
                HttpResponseMessage response = await client.GetAsync(requestString);

                /**/string request = "SERVICE: ArenaService \r\nGET: " + URLArenaService + "/" + "\r\n" + client.DefaultRequestHeaders.ToString();
                /**/string responseString = response.Headers.ToString() + "\nStatus: " + response.StatusCode.ToString();
                /**/byte[] responseMessage;

                if (response.IsSuccessStatusCode)
                {
                    /**/ responseMessage = await response.Content.ReadAsByteArrayAsync();
                    var arenaContent = await response.Content.ReadAsStringAsync();
                    arena = JsonConvert.DeserializeObject<Arena>(arenaContent);
                    if (arena == null)
                    {
                        /**/await LogQuery(request, responseString, responseMessage);
                        return NotFound();
                    }
                    /**/await LogQuery(request, responseString, responseMessage);
                    return View(arena);
                }
                else
                {
                    /**/responseMessage = Encoding.UTF8.GetBytes(response.ReasonPhrase);
                    /**/await LogQuery(request, responseString, responseMessage);
                    return Error();
                }
            }
        }


        [Route("Edite/{id}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edite([Bind("ID,ArenaName,Capacity,CityID")] Arena arena)
        {
            if (ModelState.IsValid)
            {
                //СЕРИАЛИЗУЕМ arena и посылаем на ArenaService
                var values = new JObject();
                values.Add("ID", arena.ID);
                values.Add("ArenaName", arena.ArenaName);
                values.Add("CityID", arena.CityID);
                values.Add("Capacity", arena.Capacity);

                /**/
                var corrId = string.Format("{0}{1}", DateTime.Now.Ticks, Thread.CurrentThread.ManagedThreadId);
                /**/string request;
                /**/string requestMessage = values.ToString();
                /**/byte[] responseMessage;

                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(URLArenaService);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpContent content = new StringContent(values.ToString(), Encoding.UTF8, "application/json");

                /**/string requestString = "api/arenas/" + arena.ID;

                var response = await client.PutAsJsonAsync(requestString, values);

                /**/request = "SERVICE: ArenaService \r\nPUT: " + URLArenaService + "/" + requestString + "\r\n" + client.DefaultRequestHeaders.ToString();
                /**/string responseString = response.Headers.ToString() + "\nStatus: " + response.StatusCode.ToString();

                if ((int)response.StatusCode == 500)
                {
                    string description = "There is no city with ID (" + arena.CityID + ")";
                    ResponseMessage message = new ResponseMessage();
                    message.description = description;
                    message.message = response;
                    return View("Error", message);
                }

                if (response.IsSuccessStatusCode)
                {
                    /**/responseMessage = await response.Content.ReadAsByteArrayAsync();
                    /**/await LogQuery(request, requestMessage, responseString, responseMessage);
                    return RedirectToAction(nameof(Index), new { id = 1 });
                }
                else
                {
                    /**/responseMessage = Encoding.UTF8.GetBytes(response.ReasonPhrase);
                    /**/await LogQuery(request, requestMessage, responseString, responseMessage);
                    return View(response);
                }
            }
            else
            {
                return View();
            }
        }


        [Route("Error")]
        public IActionResult Error()
        {
            return View("Error");
        }
    }
}