using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LibraryWeb.Models.Authors;
using LibraryWeb.Service;

namespace LibraryWeb.Controllers.Domain
{
    public class AuthorsController : Controller
    {
        private AuthorService _authorSrvice;

        public AuthorsController()
        {
            this._authorSrvice = new AuthorService();
        }

        // GET: Authors
        public ActionResult Index()
        {
            var authors = this._authorSrvice.GetAllAuthors();
            return View("Index", authors);
        }

        // GET: Authors/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Authors/Create
        public ActionResult Create()
        {
            if (TempData["Errors"] != null)
            {
                ModelState.AddModelError("", TempData["Errors"].ToString());
            }
            
            return View("Create");
        }

        // POST: Authors/Create
        [HttpPost]
        public ActionResult Create(AuthorModel author)
        {
            try
            {
                // TODO: Add insert logic here
                this._authorSrvice.CreateAuthor(author);
                return RedirectToAction("Index");
            }
            catch (ArgumentException ex)
            {
                TempData["Errors"] = ex.Message;
                return View();
            }
        }

        // GET: Authors/Edit/5
        public ActionResult Edit(int id)
        {
            if (TempData["Errors"] != null)
            {
                ModelState.AddModelError("", TempData["Errors"].ToString());
            }

            var author = this._authorSrvice.GetById(id);
            if (author == null)
            {
                return HttpNotFound();
            }

            return View("Edit", author);
        }

        // POST: Authors/Edit/5
        [HttpPost]
        public ActionResult Edit(AuthorModel author)
        {
            try
            {
                this._authorSrvice.Edit(author);
                return RedirectToAction("Index");
            }
            catch (ArgumentException ex)
            {
                TempData["Errors"] = ex.Message;
            }

            return RedirectToAction("Edit", author.Id);
        }

        // GET: Authors/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Authors/Delete/5
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
