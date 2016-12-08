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
        public ActionResult Index()
        {
            var bookRepo = new BookEntity();
            var books = bookRepo.GetAll();
            return View("~/Views/Books/Index.cshtml", books);
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