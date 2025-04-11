using System;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Bookstore_Group6.Models;
using System.Web;

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
            // Validate password length
            if (string.IsNullOrEmpty(plainPassword) || plainPassword.Length < 6)
            {
                ModelState.AddModelError("plainPassword", "Password must be at least 6 characters long.");
            }

            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(model.Role))
                {
                    model.Role = "User"; 
                }

                // Hash the password securely
                using (var sha256 = SHA256.Create())
                {
                    byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(plainPassword));
                    model.Password = Convert.ToBase64String(hashBytes);
                }

                // Add the new user to the database
                db.BuyerBorrowers.Add(model);
                db.SaveChanges();

                // Redirect to success page
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

        // GET: BuyerBorrower/Login
        public ActionResult Login()
        {
            // Check for the remember-me cookie
            if (Request.Cookies["UserInfo"] != null)
            {
                string userEmail = Request.Cookies["UserInfo"]["UserEmail"];
                int userId = int.Parse(Request.Cookies["UserInfo"]["UserId"]);

                var user = db.BuyerBorrowers.FirstOrDefault(u => u.Email == userEmail && u.Id == userId);
                if (user != null)
                {
                    // Set session
                    Session["UserId"] = user.Id;
                    Session["UserEmail"] = user.Email;
                    Session["UserName"] = user.Name;
                    Session["UserRole"] = user.Role;

                    return RedirectToAction("LoginSuccess", new { id = user.Id });
                }
            }

            return View();
        }

        // POST: BuyerBorrower/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Models.Login model, bool rememberMe = false)
        {
            if (ModelState.IsValid)
            {
                using (var sha256 = SHA256.Create())
                {
                    byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(model.Password));
                    string hashedPassword = Convert.ToBase64String(hashBytes);

                    var user = db.BuyerBorrowers.FirstOrDefault(u => u.Email == model.Email && u.Password == hashedPassword);

                    if (user != null)
                    {
                        // If "Remember Me" is checked, set a persistent cookie for the user
                        if (rememberMe)
                        {
                            var cookie = new HttpCookie("UserInfo")
                            {
                                ["UserId"] = user.Id.ToString(),
                                ["UserEmail"] = user.Email,
                                ["UserName"] = user.Name,
                                ["UserRole"] = user.Role.ToString(),
                                Expires = DateTime.Now.AddDays(30), // Set cookie expiration (30 days here)
                                HttpOnly = true, // Prevent access from JavaScript
                                Secure = Request.IsSecureConnection // Only send cookie over HTTPS
                            };
                            Response.Cookies.Add(cookie);  // Store in cookie
                        }
                        else
                        {
                            // Otherwise, store user info in session
                            Session["UserId"] = user.Id;
                            Session["UserEmail"] = user.Email;
                            Session["UserName"] = user.Name;
                            Session["UserRole"] = user.Role;
                        }

                        return RedirectToAction("LoginSuccess", new { id = user.Id });
                    }
                }

                ModelState.AddModelError("", "Invalid login attempt.");
            }

            return View(model);
        }

        // POST: BuyerBorrower/LoginAjax
        [HttpPost]
        public JsonResult LoginAjax(Models.Login model, bool rememberMe = false)
        {
            if (ModelState.IsValid)
            {
                using (var sha256 = SHA256.Create())
                {
                    var hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(model.Password));
                    var hashedPassword = Convert.ToBase64String(hash);

                    var user = db.BuyerBorrowers.FirstOrDefault(u => u.Email == model.Email && u.Password == hashedPassword);

                    if (user != null)
                    {
                        // If "Remember Me" is checked, set a persistent cookie for the user
                        if (rememberMe)
                        {
                            var cookie = new HttpCookie("UserInfo")
                            {
                                ["UserId"] = user.Id.ToString(),
                                ["UserEmail"] = user.Email,
                                ["UserName"] = user.Name,
                                ["UserRole"] = user.Role.ToString(),
                                Expires = DateTime.Now.AddDays(30), // Set cookie expiration (30 days here)
                                HttpOnly = true, // Prevent access from JavaScript
                                Secure = Request.IsSecureConnection // Only send cookie over HTTPS
                            };
                            Response.Cookies.Add(cookie);  // Store in cookie
                        }
                        else
                        {
                            // Otherwise, store user info in session
                            Session["UserId"] = user.Id;
                            Session["UserEmail"] = user.Email;
                            Session["UserName"] = user.Name;
                            Session["UserRole"] = user.Role;
                        }

                        return Json(new
                        {
                            success = true,
                            redirectUrl = Url.Action("LoginSuccess", "BuyerBorrower", new { id = user.Id })
                        });
                    }
                }
            }

            return Json(new { success = false, message = "Invalid email or password." });
        }

        // GET: BuyerBorrower/LoginSuccess
        public ActionResult LoginSuccess(int id)
        {
            var user = db.BuyerBorrowers.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Login");
        }

        [HttpPost]
        public JsonResult UpdateRole(int id, string role)
        {
            if (Session["UserRole"]?.ToString() != "Admin")
                return Json(new { success = false, message = "Unauthorized" });

            var user = db.BuyerBorrowers.Find(id);
            if (user != null)
            {
                user.Role = role;
                db.SaveChanges();
                return Json(new { success = true, message = $"Role updated to {role} for {user.Name}" });
            }

            return Json(new { success = false, message = "User not found" });
        }
        public ActionResult Index()
        {
            if (Session["UserRole"] == null || Session["UserRole"].ToString() != "Admin")
            {
                return new HttpUnauthorizedResult();
            }

            var users = db.BuyerBorrowers.ToList();
            return View(users);
        }

        // GET: BuyerBorrower/ForgotPassword
        public ActionResult ForgotPassword()
        {
            ViewBag.Message = TempData["Message"];
            return View();
        }

        // POST: BuyerBorrower/ForgotPassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ForgotPassword(Models.Login model)
        {
            if (ModelState.IsValid)
            {
                var user = db.BuyerBorrowers.FirstOrDefault(u => u.Email == model.Email);
                if (user != null)
                {
                    // Debug log to confirm user lookup
                    System.Diagnostics.Debug.WriteLine("✅ Found user: " + user.Email);

                    // Create a simple reset link
                    var resetLink = Url.Action("ResetPassword", "BuyerBorrower", new { email = user.Email }, protocol: Request.Url.Scheme);

                    System.Diagnostics.Debug.WriteLine("🔗 Reset link: " + resetLink);

                    try
                    {
                        MailMessage mail = new MailMessage("eamonjam@gmail.com", user.Email);
                        mail.Subject = "Password Reset";
                        mail.Body = $"Click here to reset your password: <a href='{resetLink}'>Reset Password</a>";
                        mail.IsBodyHtml = true;

                        SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587)
                        {
                            Credentials = new NetworkCredential("eamonjam@gmail.com", "lratnxkztjjdejvg"), // Use app password
                            EnableSsl = true,
                            DeliveryMethod = SmtpDeliveryMethod.Network
                        };

                        smtp.Send(mail);

                        TempData["Message"] = "✅ A password reset link has been sent to your email.";
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine("❌ Email send error: " + ex.Message);
                        TempData["Message"] = "❌ Error sending email: " + ex.Message;
                    }
                }
                else
                {
                    TempData["Message"] = "⚠️ No account with that email exists.";
                }

                return RedirectToAction("ForgotPassword");
            }

            // If validation fails, stay on the same page
            ViewBag.Message = TempData["Message"];
            return View(model);
        }


        [HttpGet]
        public ActionResult ResetPassword(int id)
        {
            var user = db.BuyerBorrowers.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }

            ViewBag.UserId = id;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = db.BuyerBorrowers.Find(model.Id);
            if (user == null)
            {
                return HttpNotFound();
            }

            using (var sha256 = SHA256.Create())
            {
                var hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(model.NewPassword));
                user.Password = Convert.ToBase64String(hash);
            }

            db.SaveChanges();
            ViewBag.Message = "Password reset successfully. You may now log in.";

            return RedirectToAction("Login");
        }

    }
}





