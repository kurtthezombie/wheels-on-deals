using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WheelsOnDeals.Models;

namespace WheelsOnDeals.Controllers
{
    public class HomeController : Controller
    {
        CAR_SELLING_Entities db = new CAR_SELLING_Entities();
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(USER_INFO user)
        {
            var checkLogin = db.USER_INFO.Where(x => x.USER_EMAIL.Equals(user.USER_EMAIL) 
            && x.USER_PASSWORD.Equals(user.USER_PASSWORD)).FirstOrDefault();
            
            if (checkLogin != null)
            {
                Session["Session_UserId"] = user.USER_IDN.ToString();
                Session["Session_UserEmail"] = user.USER_EMAIL.ToString();
                Session["Session_UserFirstName"] = checkLogin.USER_FNAME.ToString();
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.Notification = "Incorrect username and password";
            }
            return View();
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index","Home");
        }

        [HttpGet]
        public ActionResult Signup()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Signup(USER_INFO user)
        {
            if(db.USER_INFO.Any(x => x.USER_EMAIL == user.USER_EMAIL))
            {
                ViewBag.Notification = "Email is already in use.";
                return View();
            }
            else
            {
                if (ModelState.IsValid)
                {
                    db.USER_INFO.Add(user);
                    db.SaveChanges();

                    
                    return RedirectToAction("Index", "Home");
                } 
                else
                {
                    return View(user);
                }
            }   
        }

        public ActionResult ListUsers()
        {
            return View(db.USER_INFO.ToList());
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