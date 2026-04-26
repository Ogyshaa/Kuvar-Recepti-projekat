using System.Web.Mvc;

namespace Kuvar.Controllers
{
    public class BaseController : Controller
    {
        protected string CurrentUsername
        {
            get { return Session["user"] as string; }
        }

        protected bool IsLoggedIn
        {
            get { return !string.IsNullOrWhiteSpace(CurrentUsername); }
        }

        protected bool IsAdminUser
        {
            get { return Session["isAdmin"] is bool isAdmin && isAdmin; }
        }

        protected ActionResult RedirectToLogin()
        {
            return RedirectToAction("Login", "Autentikacija");
        }
    }
}
