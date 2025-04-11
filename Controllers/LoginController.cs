using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Bookstore_Group6.Models;

namespace Bookstore_Group6.Controllers
{
    public class LoginController : Controller
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Login()
        {
            return View();
        }


    }
}