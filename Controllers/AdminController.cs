using Kuvar.Models;
using Kuvar.Service;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Kuvar.Controllers
{
    public class AdminController : BaseController
    {
        private readonly KorisniciService korisniciService = new KorisniciService();
        private readonly ReceptService receptService = new ReceptService();

        public ActionResult Panel()
        {
            if (!IsAdminUser)
            {
                return RedirectToLogin();
            }

            return View(new AdminDashboardViewModel
            {
                Korisnici = korisniciService.GetAll().OrderBy(x => x.KorIme).ToList(),
                Recepti = receptService.GetAll().OrderByDescending(x => x.Objavljenj).ToList()
            });
        }

        public ActionResult DeleteUser(int id)
        {
            if (!IsAdminUser)
            {
                return RedirectToLogin();
            }

            var user = korisniciService.GetAll().FirstOrDefault(x => x.Id == id);
            if (user == null || user.KorIme.Equals("admin", StringComparison.OrdinalIgnoreCase))
            {
                return RedirectToAction("Panel");
            }

            korisniciService.Delete(id);
            receptService.DeleteByAuthor(user.KorIme);
            TempData["Success"] = "Korisnik i njegovi recepti su obrisani.";
            return RedirectToAction("Panel");
        }

        public ActionResult DeleteRecipe(int id)
        {
            if (!IsAdminUser)
            {
                return RedirectToLogin();
            }

            receptService.Delete(id);
            TempData["Success"] = "Recept je obrisan iz admin panela.";
            return RedirectToAction("Panel");
        }

        public ActionResult Report()
        {
            if (!IsAdminUser)
            {
                return RedirectToLogin();
            }

            var korisnici = korisniciService.GetAll();
            var recepti = receptService.GetAll();

            var model = new AdminReportViewModel
            {
                BrojKorisnika = korisnici.Count,
                BrojRecepata = recepti.Count,
                Kategorije = recepti
                    .GroupBy(x => x.Kategorija)
                    .Select(x => new ReceptPoKategorijiViewModel
                    {
                        NazivKategorije = x.Key,
                        BrojRecepata = x.Count()
                    })
                    .OrderByDescending(x => x.BrojRecepata)
                    .ToList(),
                NajnovijiRecepti = recepti
                    .OrderByDescending(x => x.Objavljenj)
                    .Take(10)
                    .ToList()
            };

            return View(model);
        }
    }
}
