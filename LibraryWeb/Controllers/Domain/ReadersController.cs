using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LibraryWeb.Service;
using LibraryWeb.Models.Readers;

namespace LibraryWeb.Controllers
{
    public class ReadersController : Controller
    {
        private ReaderService _readerService;
        private HistoryService _historyService;

        public ReadersController()
        {
            this._readerService = new ReaderService();
            this._historyService = new HistoryService();
        }

        // GET: Readers
        public ActionResult Index()
        {
            return View("Index", this._readerService.GetAllReaders());
        }

        // GET: Readers/Details/5
        public ActionResult Details(int id)
        {
            return View(this._historyService.GetReaderHistory(id));
        }

        // GET: Readers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Readers/Create
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

        // GET: Readers/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Readers/Edit/5
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

        // GET: Readers/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Readers/Delete/5
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
