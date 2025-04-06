using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Bookstore_Group6.Models;

namespace Bookstore_Group6.Controllers
{
    public class BooksController : Controller
    {
        // GET: Books
        public ActionResult Index()
        {
            using (var db = new ApplicationDbContext())
            {
                var books = db.Books.ToList();
                return View(books);
            }
        }

        // GET: Books/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Books/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Books book)
        {
            if (ModelState.IsValid)
            {
                using (var db = new ApplicationDbContext())
                {
                    db.Books.Add(book);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            return View(book);
        }

        // GET: Books/Edit/5
        public ActionResult Edit(int id)
        {
            using (var db = new ApplicationDbContext())
            {
                var book = db.Books.FirstOrDefault(b => b.Id == id);
                if (book == null)
                {
                    return HttpNotFound();
                }
                return View(book);
            }
        }

        // POST: Books/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Books book)
        {
            if (ModelState.IsValid)
            {
                using (var db = new ApplicationDbContext())
                {
                    db.Entry(book).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            return View(book);
        }

        // GET: Books/Details/5
        public ActionResult Details(int id)
        {
            using (var db = new ApplicationDbContext())
            {
                var book = db.Books.FirstOrDefault(b => b.Id == id);
                if (book == null)
                {
                    return HttpNotFound();
                }
                return View(book);
            }
        }

        // GET: Books/Delete/5
        public ActionResult Delete(int id)
        {
            using (var db = new ApplicationDbContext())
            {
                var book = db.Books.FirstOrDefault(b => b.Id == id);
                if (book == null)
                {
                    return HttpNotFound();
                }
                return View(book);
            }
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            using (var db = new ApplicationDbContext())
            {
                var book = db.Books.FirstOrDefault(b => b.Id == id);
                if (book == null)
                {
                    return HttpNotFound();
                }

                db.Books.Remove(book);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
        }

        // AJAX Search Functionality
        public JsonResult SearchBooks(string query)
        {
            using (var db = new ApplicationDbContext())
            {
                var results = db.Books
                                .Where(b => b.Name.Contains(query) ||
                                            b.Author.Contains(query) ||
                                            b.ISBN.Contains(query))
                                .Select(b => new { b.Id, b.Name, b.Author })
                                .ToList();

                return Json(results, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult SaveSearch(string query)
        {
            // Check if the query is empty or just whitespace
            if (string.IsNullOrWhiteSpace(query))
            {
                // Return a validation error if the query is empty
                return Json(new { success = false, message = "Search query is required." });
            }

            try
            {
                // Create a new SearchHistory instance
                var searchHistory = new SearchHistory
                {
                    Query = query,
                    SearchDate = DateTime.Now
                };

                using (var db = new ApplicationDbContext())
                {
                    // Add the searchHistory to the database
                    db.SearchHistories.Add(searchHistory);
                    db.SaveChanges();
                }

                // Return success if everything goes well
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                // Handle any exceptions that might occur during saving
                return Json(new { success = false, message = "An error occurred while saving the search history." });
            }
        }

    }
}



