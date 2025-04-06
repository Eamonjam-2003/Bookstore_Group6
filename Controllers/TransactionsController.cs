using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Bookstore_Group6.Models;

namespace Bookstore_Group6.Controllers
{
    public class TransactionsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();  // Declare the context at the class level

        // GET: Transactions
        public ActionResult Index()
        {
            var transactions = db.Transactions.Include("Book").Include("Client").ToList();

            return View(transactions);
        }

        // GET: Transactions/Create
        public ActionResult Create()
        {
            ViewBag.BookId = new SelectList(db.Books, "Id", "Name");
            ViewBag.ClientId = new SelectList(db.Clients, "Id", "Name");
            return View();
        }

        // POST: Transactions/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Transactions transaction)
        {
            if (ModelState.IsValid)
            {
                db.Transactions.Add(transaction);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.BookId = new SelectList(db.Books, "Id", "Name", transaction.BookId);
            ViewBag.ClientId = new SelectList(db.Clients, "Id", "Name", transaction.ClientId);
            return View(transaction);
        }

        // GET: Transactions/Edit/5
        public ActionResult Edit(int id)
        {
            using (var db = new ApplicationDbContext())
            {
                var transaction = db.Transactions.FirstOrDefault(t => t.Id == id);
                if (transaction == null)
                {
                    return HttpNotFound();
                }

                ViewBag.BookId = new SelectList(db.Books, "Id", "Name", transaction.BookId);
                ViewBag.ClientId = new SelectList(db.Clients, "Id", "Name", transaction.ClientId);
                return View(transaction);
            }
        }

        // POST: Transactions/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Transactions transaction)
        {
            using (var db = new ApplicationDbContext())
            {
                if (ModelState.IsValid)
                {
                    db.Entry(transaction).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                ViewBag.BookId = new SelectList(db.Books, "Id", "Name", transaction.BookId);
                ViewBag.ClientId = new SelectList(db.Clients, "Id", "Name", transaction.ClientId);
                return View(transaction);
            }
        }

        // GET: Transactions/Details/5
        public ActionResult Details(int id)
        {
            var transaction = db.Transactions.Include("Book").Include("Client").FirstOrDefault(t => t.Id == id);
            if (transaction == null)
            {
                return HttpNotFound();
            }
            return View(transaction);
        }

        // GET: Transactions/Delete/5
        public ActionResult Delete(int id)
        {
            var transaction = db.Transactions.Include("Book").Include("Client").FirstOrDefault(t => t.Id == id);
            if (transaction == null)
            {
                return HttpNotFound();
            }

            return View(transaction);
        }

        // POST: Transactions/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, FormCollection collection)
        {
            var transaction = db.Transactions.Include("Book").Include("Client").FirstOrDefault(t => t.Id == id);
            if (transaction == null)
            {
                return HttpNotFound();
            }

            db.Transactions.Remove(transaction);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // Dispose method for cleaning up the context
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

