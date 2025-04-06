using System;
using System.Linq;
using System.Web.Mvc;
using Bookstore_Group6.Models;

namespace Bookstore_Group6.Controllers
{
    public class BuyerBorrowerController : Controller
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SignUp(BuyerBorrower model)
        {
            if (ModelState.IsValid)
            {
                db.BuyerBorrowers.Add(model);
                db.SaveChanges();
                return RedirectToAction("Success", new { id = model.Id });
            }
            return View(model);
        }

        public ActionResult Success(int id)
        {
            var buyerBorrower = db.BuyerBorrowers.Find(id);
            if (buyerBorrower == null)
            {
                return HttpNotFound();
            }
            return View(buyerBorrower);
        }
    }
}



