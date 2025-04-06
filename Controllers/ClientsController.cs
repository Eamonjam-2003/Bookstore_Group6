using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Bookstore_Group6.Models;

namespace Bookstore_Group6.Controllers
{
    public class ClientsController : Controller
    {
        // GET: Clients
        public ActionResult Index()
        {
            using (var db = new ApplicationDbContext())
            {
                var clients = db.Clients.ToList();
                return View(clients);
            }
        }

        // GET: Clients/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Clients/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Clients client)
        {
            if (ModelState.IsValid)
            {
                using (var db = new ApplicationDbContext())
                {
                    db.Clients.Add(client);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            return View(client);
        }

        // GET: Clients/Edit/5
        public ActionResult Edit(int id)
        {
            using (var db = new ApplicationDbContext())
            {
                var client = db.Clients.FirstOrDefault(c => c.Id == id);
                if (client == null)
                {
                    return HttpNotFound();
                }
                return View(client);
            }
        }

        // POST: Clients/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Clients client)
        {
            if (ModelState.IsValid)
            {
                using (var db = new ApplicationDbContext())
                {
                    db.Entry(client).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            return View(client);
        }

        // GET: Clients/Details/5
        public ActionResult Details(int id)
        {
            using (var db = new ApplicationDbContext())
            {
                var client = db.Clients.FirstOrDefault(c => c.Id == id);
                if (client == null)
                {
                    return HttpNotFound();
                }
                return View(client);
            }
        }

        // GET: Clients/Delete/5
        public ActionResult Delete(int id)
        {
            using (var db = new ApplicationDbContext())
            {
                var client = db.Clients.FirstOrDefault(c => c.Id == id);
                if (client == null)
                {
                    return HttpNotFound();
                }
                return View(client);
            }
        }

        // POST: Clients/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, FormCollection collection)
        {
            using (var db = new ApplicationDbContext())
            {
                var client = db.Clients.FirstOrDefault(c => c.Id == id);
                if (client == null)
                {
                    return HttpNotFound();
                }
                db.Clients.Remove(client);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
        }
    }
}
