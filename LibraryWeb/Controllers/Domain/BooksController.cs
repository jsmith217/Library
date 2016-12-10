using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using LibraryWeb.Repository;
using LibraryWeb.Service;
using LibraryWeb.Models.Books;

namespace LibraryWeb.Controllers
{
    public class BooksController : Controller
    {
        private BookService _bookService;

        public BooksController()
        {
            this._bookService = new BookService();
        }

        // GET: Book
        public ActionResult Index(string orderColumn, bool? viewAvailableOnly, int? page)
        {
            var books = new List<BookModel>();

            ViewBag.CurrentSort = orderColumn;
            ViewBag.TitleSortParm = String.IsNullOrEmpty(orderColumn) ? "Title desc" : "Title";
            ViewBag.TotalSortParm = orderColumn == "Total" ? "Total desc" : "Total";
            ViewBag.AvailableSortParm = orderColumn == "Available" ? "Available desc" : "Available";

            if (viewAvailableOnly.HasValue && viewAvailableOnly.Value)
            {
                books = this._bookService.GetAllAvailableBooks();
            }
            else
            {
                books = this._bookService.GetAllBooks(orderColumn);
            }

            int pageSize = 5;
            return View("Index", books.ToPagedList(page ?? 1, pageSize));
        }

        [Authorize]
        public EmptyResult Take(int id)
        {
            //return View();
            return new EmptyResult();
        }

        // GET: Book/Details/5
        public ActionResult Details(int id)
        {
            return View(this._bookService.GetById(id));
        }

        // GET: Book/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Book/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Book/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Book/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Book/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Book/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
