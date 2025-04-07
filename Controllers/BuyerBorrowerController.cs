using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
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
        public ActionResult SignUp(BuyerBorrower model, string plainPassword)
        {
            if (string.IsNullOrEmpty(plainPassword) || plainPassword.Length < 6)
            {
                ModelState.AddModelError("plainPassword", "Password must be at least 6 characters long.");
            }

            if (ModelState.IsValid)
            {
                // Hash the password securely
                using (var sha256 = SHA256.Create())
                {
                    byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(plainPassword));
                    model.Password = Convert.ToBase64String(hashBytes);
                }

                db.BuyerBorrowers.Add(model);
                db.SaveChanges();

                return RedirectToAction("Success", new { id = model.Id });
            }

            // Optional debug logging for ModelState errors
            foreach (var key in ModelState.Keys)
            {
                var errors = ModelState[key].Errors;
                foreach (var error in errors)
                {
                    System.Diagnostics.Debug.WriteLine($"{key}: {error.ErrorMessage}");
                }
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





