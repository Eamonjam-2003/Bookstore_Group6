using System;
using System.Web;
using System.Web.Mvc;

namespace Bookstore_Group6.Filters
{

    public class AdminOnlyAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var roleFromCookie = httpContext.Request.Cookies["UserInfo"]?["UserRole"];
            var roleFromSession = httpContext.Session["UserRole"]?.ToString();

            string role = roleFromCookie ?? roleFromSession;

            return role != null && role.Equals("Admin", StringComparison.OrdinalIgnoreCase);
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            // Redirect unauthorized users to a custom page or login
            filterContext.Result = new RedirectResult("~/Home/AccessDenied");
        }
    }
}
