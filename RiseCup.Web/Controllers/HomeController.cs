using System;
using System.Collections.Generic;
using System.Web.Mvc;
using RiseCup.Database;
using RiseCup.Domain.Models;

namespace RiseCup.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly RiseCupContext _db;

        public HomeController()
        {
            _db = new RiseCupContext();
        }
        
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            return View();
        }

        public ActionResult Testimonial()
        {
            return View();
        }

        public ActionResult Products()
        {
            return View();
        }
    }
}