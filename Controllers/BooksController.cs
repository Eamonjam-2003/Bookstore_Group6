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

        [Authorize(Roles = "Admin")]
        // GET: Books/Create
        public ActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
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

        [Authorize(Roles = "Admin")]
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

        [Authorize(Roles = "Admin")]
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

        [Authorize(Roles = "Admin")]
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

        [Authorize(Roles = "Admin")]
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
        // GET: Books/Buy/5
        public ActionResult Buy(int id)
        {
            using (var db = new ApplicationDbContext())
            {
                var book = db.Books.FirstOrDefault(b => b.Id == id);
                if (book == null)
                {
                    return HttpNotFound();
                }

                // Check if the book is already bought
                if (book.IsBought)
                {
                    // Redirect to the "AlreadyBought" action if the book is bought
                    return RedirectToAction("AlreadyBought", new { id = id });
                }

                // Check if the book is already borrowed
                if (book.IsBorrowed)
                {
                    // Redirect to the "AlreadyBorrowed" action if the book is borrowed
                    return RedirectToAction("AlreadyBorrowed", new { id = id });
                }

                // If the book is neither bought nor borrowed, pass it to the Buy view
                return View(book);
            }
        }

        // POST: Books/CompleteBuy/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CompleteBuy(int id)
        {
            using (var db = new ApplicationDbContext())
            {
                var book = db.Books.FirstOrDefault(b => b.Id == id);
                if (book == null)
                {
                    return HttpNotFound();
                }

                // Check if the book is already bought
                if (book.IsBought)
                {
                    // Redirect to the "AlreadyBought" action if the book is already bought
                    return RedirectToAction("AlreadyBought", new { id = id });
                }

                // Check if the book is already borrowed
                if (book.IsBorrowed)
                {
                    // Redirect to the "AlreadyBorrowed" action if the book is borrowed
                    return RedirectToAction("AlreadyBorrowed", new { id = id });
                }

                // Mark the book as bought
                book.IsBought = true;

                // Save changes to the database
                db.SaveChanges();

                // Redirect to the BuySuccess page after confirming the purchase
                return RedirectToAction("BuySuccess", new { id = id });
            }
        }



        // GET: Books/BuySuccess/5
        public ActionResult BuySuccess(int id)
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



        // GET: Books/Borrow/5
        public ActionResult Borrow(int id)
        {
            using (var db = new ApplicationDbContext())
            {
                var book = db.Books.FirstOrDefault(b => b.Id == id);
                if (book == null)
                {
                    return HttpNotFound();
                }

                // You can pass the book to the Borrow view
                return View(book);
            }
        }
        // GET: Books/AlreadyBought/5
        public ActionResult AlreadyBought(int id)
        {
            using (var db = new ApplicationDbContext())
            {
                var book = db.Books.FirstOrDefault(b => b.Id == id);
                if (book == null)
                {
                    return HttpNotFound();
                }

                // Pass the book to the view
                ViewBag.Message = "This book has already been bought.";
                return View(book);
            }
        }

        // POST: Books/CompleteBorrow/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CompleteBorrow(int id)
        {
            using (var db = new ApplicationDbContext())
            {
                // Retrieve the book from the database
                var book = db.Books.FirstOrDefault(b => b.Id == id);

                // If the book is not found, return a 404 error
                if (book == null)
                {
                    return HttpNotFound();
                }

                // Check if the book is already borrowed
                if (book.IsBorrowed)
                {
                    // Redirect to the "AlreadyBorrowed" action
                    return RedirectToAction("AlreadyBorrowed", new { id = id });
                }

                // Check if the book is already bought
                if (book.IsBought)
                {
                    // Redirect to the "AlreadyBought" action
                    return RedirectToAction("AlreadyBought", new { id = id });
                }

                // Set the IsBorrowed field to true to indicate the book is borrowed
                book.IsBorrowed = true;

                // Save changes to the database
                db.SaveChanges();

                // Redirect to the success page after confirming the borrow
                return RedirectToAction("BorrowSuccess", new { id = id });
            }
        }


        // GET: Books/AlreadyBorrowed/5
        public ActionResult AlreadyBorrowed(int id)
        {
            using (var db = new ApplicationDbContext())
            {
                var book = db.Books.FirstOrDefault(b => b.Id == id);
                if (book == null)
                {
                    return HttpNotFound();
                }

                // Pass the book to the view
                ViewBag.Message = "This book has already been borrowed.";
                return View(book);
            }
        }

    }
}



