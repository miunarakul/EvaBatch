using EvaBatch.Filter;
using EvaBatch.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EvaBatch.Controllers
{
    [Auth]
    public class BBController : Controller
    {
        LoginController loginCT = new LoginController();
        // GET: BB
        public ActionResult Index()
        {
            return View();
        }

        // GET: BB/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: BB/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: BB/Create
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

        // GET: BB/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: BB/Edit/5
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

        // GET: BB/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: BB/Delete/5
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
