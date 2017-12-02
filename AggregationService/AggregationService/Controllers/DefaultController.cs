using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace AggregationService.Controllers
{
    public class DefaultController : Controller
    {
        [HttpGet("{id?}")]
        public IActionResult Index(int? i)
        {
            return View();
        }

        //public IActionResult Index()
        //{
        //    return View();
        //}
    }
}