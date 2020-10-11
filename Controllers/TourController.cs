using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace IFTurist.Controllers
{
    public class TourController : Controller
    {
        public IActionResult Edit()
        {
            return View();
        }
        public IActionResult Delete()
        {
            return View();
        }

        [HttpGet]
        public IActionResult AddTour()
        {
            return View();
        }

    }
}
