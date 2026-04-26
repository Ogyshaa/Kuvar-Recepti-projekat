using Kuvar.Models;
using Kuvar.Service;
using System;
using System.Web.Mvc;

namespace Kuvar.Controllers
{
    public class AutentikacijaController : BaseController
    {
        private readonly KorisniciService service = new KorisniciService();

        public ActionResult Login()
        {
            return View(new LoginViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = service.Login(model.KorIme, model.Lozinka);

            if (user != null)
            {
                Session["user"] = user.KorIme;
                Session["isAdmin"] = user.KorIme.Equals("admin", StringComparison.OrdinalIgnoreCase);
                return RedirectToAction("Index", "Recept");
            }

            ViewBag.Error = "Pogresni podaci za prijavu.";
            return View(model);
        }

        public ActionResult Register()
        {
            return View(new Korisnik { DatumRodjenja = DateTime.Now.AddYears(-18) });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(Korisnik k)
        {
            if (!ModelState.IsValid)
            {
                return View(k);
            }

            try
            {
                service.Register(k);
                TempData["Success"] = "Registracija je uspesna. Sada se prijavi.";
                return RedirectToAction("Login");
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(k);
            }
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
