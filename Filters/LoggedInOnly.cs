using System.Web;
using System.Web.Mvc;

namespace Bookstore_Group6.Filters
{
    public class LoggedInOnlyAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            // Check for either session or cookie-based login
            var sessionUserId = httpContext.Session["UserId"];
            var cookie = httpContext.Request.Cookies["UserInfo"];

            return sessionUserId != null || (cookie != null && !string.IsNullOrEmpty(cookie["UserId"]));
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectResult("~/BuyerBorrower/Login");
        }
    }
}
