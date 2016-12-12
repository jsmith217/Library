using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using LibraryWeb.Repository;
using LibraryWeb.Service;
using LibraryWeb.Models.Books;
using LibraryWeb.Models.Readers;
using LibraryWeb.Models.History;

namespace LibraryWeb.Controllers
{
    public class BooksController : Controller
    {
        private BookService _bookService;
        private HistoryService _historyService;
        private ReaderService _readerService;

        public BooksController()
        {
            this._bookService = new BookService();
            this._historyService = new HistoryService();
            this._readerService = new ReaderService();
        }

        //[Authorize(Roles = "User, Admin")]
        // GET: Book
        public ActionResult Index(string orderColumn, bool? viewAvailableOnly, int? page)
        {
           try
            {
                if (TempData["Errors"] != null)
                {
                    ModelState.AddModelError("", TempData["Errors"].ToString());
                }

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
            catch (ArgumentException ex)
            {
                ModelState.AddModelError("", ex.Message);
            }

            return View();
        }
        
        [HttpGet]
        public ActionResult Take(int id)
        {
            //return View();
            try
            {
                string email = User.Identity.Name;
                BookModel book = this._bookService.GetById(id);
                ReaderModel reader = this._readerService.GetByDetails(email);
                this._historyService.TakeBook(new HistoryModel { Book = book, Reader = reader, DateTaken = DateTime.Now });

                ViewBag.EmailMessage = "Email notification has been sent.";
            }
            catch (ArgumentException ex)
            {
                //ModelState.AddModelError("", ex.Message);
                TempData["Errors"] = ex.Message;
            }
            return RedirectToAction("Index");
        }

        // Returns book by id
        public ActionResult Return(int id)
        {
            try
            {
                string email = User.Identity.Name;
                BookModel book = this._bookService.GetById(id);
                ReaderModel reader = this._readerService.GetByDetails(email);
            }
            catch (ArgumentException ex)
            {
                //ModelState.AddModelError("", ex.Message);
                TempData["Errors"] = ex.Message;
            }
            return View();
        }


        // GET: Book/Details/5
        public ActionResult Details(int id)
        {
            BookModel book = null;
            try
            {
                book = this._historyService.GetBookHistory(id);
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError("Arg", ex.Message);
            }

            return View("History", book);
        }
        
        // GET: Book/History/5
        public ActionResult History(int id)
        {
            return View(this._historyService.GetBookHistory(id));
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
            if (TempData["Errors"] != null)
            {
                ModelState.AddModelError("", TempData["Errors"].ToString());
            }

            var book = this._bookService.GetById(id);
            if (book == null)
            {
                return HttpNotFound();
            }

            return View("Edit", book);
        }

        [HttpPost]
        public ActionResult Edit(BookModel book)
        {
            try
            {
                this._bookService.EditQuantity(book);
                return RedirectToAction("Index");
            }
            catch (ArgumentException ex)
            {
                TempData["Errors"] = ex.Message;
            }

            return RedirectToAction("Edit", book.Id);
        }

        // POST: Book/Edit/5
        /*[HttpPost]
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
        */
        // GET: Book/Delete/5
        public ActionResult Delete(int id)
        {
            try
            {
                this._bookService.Delete(id);
            }
            catch (ArgumentException ex)
            {
                TempData["Errors"] = ex.Message;
            }

            return RedirectToAction("Index");
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
