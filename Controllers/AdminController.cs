using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Bookstore_Group6.Models;

namespace Bookstore_Group6.Controllers
{
    public class AdminController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // [Authorize(Roles = "Admin")] // Uncomment this once login is working
        public ActionResult AdminDashboard()
        {
            var buyers = db.BuyerBorrowers.ToList();
            var clients = db.Clients.ToList();
            return View(Tuple.Create(buyers, clients));
        }

        [HttpPost]
        public ActionResult DeleteBuyer(int id)
        {
            var buyer = db.BuyerBorrowers.Find(id);
            if (buyer != null)
            {
                db.BuyerBorrowers.Remove(buyer);
                db.SaveChanges();
            }
            return RedirectToAction("AdminDashboard");
        }

        [HttpPost]
        public ActionResult DeleteClient(int id)
        {
            var client = db.Clients.Find(id);
            if (client != null)
            {
                db.Clients.Remove(client);
                db.SaveChanges();
            }
            return RedirectToAction("AdminDashboard");
        }
    }
}
