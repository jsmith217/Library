using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LibraryWeb.Models.Books;
using LibraryWeb.Models.Authors;
using LibraryWeb.Repository;

namespace LibraryWeb.Controllers
{
    public class HomeController : Controller
    {
       // [Authorize (Roles = "Admin")]
        public ActionResult Index()
        {
            return RedirectToAction("Index", "Books");
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
    }
}