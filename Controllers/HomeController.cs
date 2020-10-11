using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using IFTurist.Models;
using IFTurist.Models.TourModel;

namespace IFTurist.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            Tour tour = new Tour() { Id = 1, ImageLink = "/img/tours/yaremche/yaremche.jpg", Name = "Яремче" }; // for test
            Tour tour2 = new Tour() { Id = 1, ImageLink = "/img/tours/yaremche/yaremche.jpg", Name = "122" };
            Tour tour3 = new Tour() { Id = 1, ImageLink = "/img/tours/yaremche/yaremche.jpg", Name = "33" };
            Tour tour4 = new Tour() { Id = 1, ImageLink = "/img/tours/yaremche/yaremche.jpg", Name = "33" };
            Tour tour5 = new Tour() { Id = 1, ImageLink = "/img/tours/yaremche/yaremche.jpg", Name = "33" };
            Tour tour6 = new Tour() { Id = 1, ImageLink = "/img/tours/yaremche/yaremche.jpg", Name = "33" };
            Tour tour7 = new Tour() { Id = 1, ImageLink = "/img/tours/yaremche/yaremche.jpg", Name = "33" };
            Tour tour8 = new Tour() { Id = 1, ImageLink = "/img/tours/yaremche/yaremche.jpg", Name = "33" };
            Tour tour9 = new Tour() { Id = 1, ImageLink = "/img/tours/yaremche/yaremche.jpg", Name = "33" };
            Tour tour10 = new Tour() { Id = 1, ImageLink = "/img/tours/yaremche/yaremche.jpg", Name = "33" };
            List<Tour> tours = new List<Tour> { tour, tour2, tour3, tour4, tour5, tour6, tour7, tour8, tour9, tour10 };      
            return View(tours);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
