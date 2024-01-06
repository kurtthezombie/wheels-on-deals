using Antlr.Runtime.Tree;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WheelsOnDeals.Models;

namespace WheelsOnDeals.Controllers
{
    public class HomeController : Controller
    {
        AppDbContext db = new AppDbContext();

        #region Homepage

        public ActionResult Index()
        {
            var carListingsWithOwners = db.CarListings
                .Include(cl => cl.USER_INFO)
                .ToList();
            return View(carListingsWithOwners);
        }

        [HttpGet]
        public ActionResult ListingDetails(int id)
        {
            var data = db.CarListings
                .Include(cl => cl.USER_INFO)
                .FirstOrDefault(cl => cl.CarID == id);

            if (data == null)
            {
                return HttpNotFound();
            }
            return View(data);
        }

        #endregion

        #region Logging in and out
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
                Session["Session_UserId"] = checkLogin.USER_IDN;
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

        #endregion

        #region Listings

        [HttpGet]
        public ActionResult MyListing()
        {
            var ownerId = Convert.ToInt32(Session["Session_UserId"]);
            var listings = db.CarListings.Where(listing => listing.OwnerID == ownerId).ToList();
            return View(listings);
        }

        [HttpGet]
        public ActionResult CreateListing()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateListing(CarListing listing)
        {
            //create fileName and extension
            string fileName = Path.GetFileNameWithoutExtension(listing.ImageFile.FileName);
            string extension = Path.GetExtension(listing.ImageFile.FileName);

            fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;

            listing.ImageUrl = "../Images/" + fileName;

            if (ModelState.IsValid)
            {
                
                fileName = Path.Combine(Server.MapPath("../Images/"), fileName);
                listing.ImageFile.SaveAs(fileName);

                db.CarListings.Add(listing);
                db.SaveChanges();

                return RedirectToAction("MyListing","Home");
            }
            else
            {
                return View(listing);
            }
        }

        [HttpGet]
        public ActionResult EditListing(int id)
        {
            var data = db.CarListings.Where(x => x.CarID == id).FirstOrDefault();

            if (data == null)
            {
                return HttpNotFound();
            }

            return View(data);
        }

        [HttpPost]
        public ActionResult EditListing(CarListing listing)
        {
            if (ModelState.IsValid)
            {
                var data = db.CarListings.Where(x => x.CarID.Equals(listing.CarID)).FirstOrDefault();
                
                if (data != null)
                {
                    data.Make = listing.Make;
                    data.Model = listing.Model;
                    data.CarYear = listing.CarYear;
                    data.Color = listing.Color;
                    data.Price = listing.Price;
                    data.Mileage = listing.Mileage;
                    data.CarDescription = listing.CarDescription;
                    data.CreatedAt = DateTime.Now;

                    db.SaveChanges();

                    return RedirectToAction("MyListing");
                }
                return HttpNotFound();
            }
            return View("EditListing", listing);
        }

        public ActionResult DeleteListing(int id)
        {
            var data = db.CarListings.Find(id);

            if (data == null)
            {
                return HttpNotFound();
            }
            db.CarListings.Remove(data);
            db.SaveChanges();

            return RedirectToAction("MyListing");
        }

        [HttpGet]
        public ActionResult MyListingDetails(int id)
        {
            var data = db.CarListings.Find(id);

            if (data == null)
            {
                return HttpNotFound();
            }

            return View(data);
        }
        #endregion

        #region Profile

        [HttpGet]
        public ActionResult MyProfile(int id)
        {
            var data = db.USER_INFO.Find(id);

            if (data == null)
            {
                return HttpNotFound();
            } 

            return View(data);
        }

        [HttpGet]
        public ActionResult EditProfile(int id)
        {
            var data = db.USER_INFO.Where(x => x.USER_IDN == id).FirstOrDefault();

            if(data == null)
            {
                return HttpNotFound();
            }

            return View(data);
        }

        [HttpPost]
        public ActionResult EditProfile(USER_INFO user)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var data = db.USER_INFO.Where(x => x.USER_IDN == user.USER_IDN).FirstOrDefault();

                    if (data != null)
                    {
                        data.USER_EMAIL = user.USER_EMAIL;
                        data.USER_FNAME = user.USER_FNAME;
                        data.USER_LNAME = user.USER_LNAME;
                        data.USER_PASSWORD = user.USER_PASSWORD;
                        data.ConfirmPassword = user.ConfirmPassword;

                        db.SaveChanges();

                        if (!string.IsNullOrEmpty(user.USER_EMAIL))
                        {
                            Session["Session_UserFirstName"] = user.USER_FNAME;
                            Session["Session_UserEmail"] = user.USER_EMAIL;
                        }

                        return RedirectToAction("MyProfile", new { id = user.USER_IDN } );
                    }
                    return HttpNotFound();
                }
                return View("EditProfile", user);
            }
            catch(DbEntityValidationException ex)
            {
                // Handle validation errors
                foreach (var validationErrors in ex.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        ModelState.AddModelError(validationError.PropertyName, validationError.ErrorMessage);
                    }
                }

                // If there are validation errors, return to the view with validation errors
                return View("EditProfile", user);
            }
            
        }

        #endregion

        #region Others

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

        #endregion
    }
}