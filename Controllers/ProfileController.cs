using Kuvar.Models;
using Kuvar.Service;
using System.Linq;
using System.Web.Mvc;

namespace Kuvar.Controllers
{
    public class ProfileController : BaseController
    {
        private readonly KorisniciService korService = new KorisniciService();
        private readonly ReceptService recService = new ReceptService();

        public ActionResult Index()
        {
            if (!IsLoggedIn)
            {
                return RedirectToLogin();
            }

            var user = korService.GetAll().FirstOrDefault(x => x.KorIme == CurrentUsername);
            var recipes = recService.GetAll().Where(x => x.Autor == CurrentUsername).ToList();

            return View("ProfileView", new ProfilePregledRecepta
            {
                PrijavljeniKorisnik = user,
                ObjavljeniRecipti = recipes
            });
        }

        public ActionResult Edit()
        {
            if (!IsLoggedIn)
            {
                return RedirectToLogin();
            }

            var user = korService.GetAll().FirstOrDefault(x => x.KorIme == CurrentUsername);

            if (user == null)
            {
                return RedirectToLogin();
            }

            return View(new ProfileEditViewModel
            {
                KorIme = user.KorIme,
                Email = user.Email,
                DatumRodjenja = user.DatumRodjenja
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ProfileEditViewModel model)
        {
            if (!IsLoggedIn)
            {
                return RedirectToLogin();
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = korService.GetAll().FirstOrDefault(x => x.KorIme == CurrentUsername);

            if (user == null)
            {
                return RedirectToLogin();
            }

            try
            {
                var oldUsername = user.KorIme;
                if (IsAdminUser)
                {
                    model.KorIme = "admin";
                }

                user.KorIme = model.KorIme;
                user.Email = model.Email;
                user.DatumRodjenja = model.DatumRodjenja;

                korService.Update(user);

                if (oldUsername != model.KorIme)
                {
                    recService.UpdateUsernameReferences(oldUsername, model.KorIme);
                }

                Session["user"] = model.KorIme;
                Session["isAdmin"] = model.KorIme == "admin";
                TempData["Success"] = "Profil je uspesno azuriran.";
                return RedirectToAction("Index");
            }
            catch (System.InvalidOperationException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(model);
            }
        }
    }
}
