using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WheelsOnDeals.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Login()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Signup()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Signup(USER_INFO model)
        {
            /*/insert something idk
            _context.USER_INFO.Add(model);
            _context.SaveChanges();
            ViewBag.Message = "Data inserted successfully.";*/
            return RedirectToAction("Index");
        }

        public ActionResult Contact()
        {
            return View();
        }

        public ActionResult About()
        {
            return View();
        }

    }
}