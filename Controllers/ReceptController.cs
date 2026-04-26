using Kuvar.Models;
using Kuvar.Service;
using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kuvar.Controllers
{
    public class ReceptController : BaseController
    {
        private readonly ReceptService service = new ReceptService();

        public ActionResult Index(string pretraga, string kategorija, string sortiranje)
        {
            if (!IsLoggedIn)
            {
                return RedirectToLogin();
            }

            var sviRecepti = service.GetAll();
            var recepti = sviRecepti.AsQueryable();

            if (!string.IsNullOrWhiteSpace(pretraga))
            {
                recepti = recepti.Where(x =>
                    x.Naziv.IndexOf(pretraga, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    x.Sastojci.IndexOf(pretraga, StringComparison.OrdinalIgnoreCase) >= 0);
            }

            if (!string.IsNullOrWhiteSpace(kategorija))
            {
                recepti = recepti.Where(x => x.Kategorija == kategorija);
            }

            switch (sortiranje)
            {
                case "naziv":
                    recepti = recepti.OrderBy(x => x.Naziv);
                    break;
                case "kategorija":
                    recepti = recepti.OrderBy(x => x.Kategorija);
                    break;
                default:
                    recepti = recepti.OrderByDescending(x => x.Objavljenj).ThenByDescending(x => x.Id);
                    break;
            }

            var model = new ReceptIndexViewModel
            {
                Recepti = recepti.ToList(),
                Pretraga = pretraga,
                Kategorija = kategorija,
                Sortiranje = sortiranje,
                Kategorije = sviRecepti.Select(x => x.Kategorija).Distinct().OrderBy(x => x).ToList()
            };

            return View(model);
        }

        public ActionResult Create()
        {
            if (!IsLoggedIn)
            {
                return RedirectToLogin();
            }

            return View(new Recept());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Recept r, HttpPostedFileBase slikaFajl)
        {
            if (!IsLoggedIn)
            {
                return RedirectToLogin();
            }

            if (!ModelState.IsValid)
            {
                return View(r);
            }

            r.Autor = CurrentUsername;
            r.Slika = SacuvajSliku(slikaFajl);
            service.Add(r);
            return RedirectToAction("Index");
        }

        public ActionResult Details(int id)
        {
            if (!IsLoggedIn)
            {
                return RedirectToLogin();
            }

            var recept = service.GetById(id);
            if (recept == null)
            {
                return HttpNotFound();
            }

            return View(recept);
        }

        public ActionResult Edit(int id)
        {
            if (!IsLoggedIn)
            {
                return RedirectToLogin();
            }

            var recept = service.GetById(id);
            if (recept == null)
            {
                return HttpNotFound();
            }

            if (recept.Autor != CurrentUsername)
            {
                return RedirectToAction("Index");
            }

            return View(recept);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Recept r, HttpPostedFileBase slikaFajl)
        {
            if (!IsLoggedIn)
            {
                return RedirectToLogin();
            }

            if (!ModelState.IsValid)
            {
                return View(r);
            }

            var postojeci = service.GetById(r.Id);
            if (postojeci == null || postojeci.Autor != CurrentUsername)
            {
                return RedirectToAction("Index");
            }

            r.Autor = postojeci.Autor;
            r.Objavljenj = postojeci.Objavljenj;
            r.Slika = SacuvajSliku(slikaFajl, postojeci.Slika);
            service.Update(r);
            return RedirectToAction("Details", new { id = r.Id });
        }

        public ActionResult Delete(int id)
        {
            if (!IsLoggedIn)
            {
                return RedirectToLogin();
            }

            var recept = service.GetById(id);
            if (recept != null && (recept.Autor == CurrentUsername || IsAdminUser))
            {
                service.Delete(id);
            }

            return RedirectToAction("Index");
        }

        public ActionResult ToggleFavorite(int id)
        {
            if (!IsLoggedIn)
            {
                return RedirectToLogin();
            }

            service.ToggleFavorite(id, CurrentUsername);
            return RedirectToAction("Index");
        }

        public ActionResult Favorites()
        {
            if (!IsLoggedIn)
            {
                return RedirectToLogin();
            }

            var fav = service.GetAll().Where(x => x.Omiljeno.Contains(CurrentUsername)).ToList();
            return View(fav);
        }

        public ActionResult ExportXml()
        {
            if (!IsAdminUser)
            {
                return RedirectToLogin();
            }

            return File(System.Text.Encoding.UTF8.GetBytes(service.ExportAsXml()), "application/xml", "recepti.xml");
        }

        private string SacuvajSliku(HttpPostedFileBase slikaFajl, string postojecaSlika = null)
        {
            if (slikaFajl == null || slikaFajl.ContentLength == 0)
            {
                return postojecaSlika;
            }

            var dozvoljeneEkstenzije = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
            var ekstenzija = Path.GetExtension(slikaFajl.FileName);

            if (string.IsNullOrWhiteSpace(ekstenzija) ||
                !dozvoljeneEkstenzije.Contains(ekstenzija, StringComparer.OrdinalIgnoreCase))
            {
                return postojecaSlika;
            }

            var uploadFolder = Server.MapPath("~/Content/Uploads");
            Directory.CreateDirectory(uploadFolder);

            var fileName = Guid.NewGuid().ToString("N") + ekstenzija.ToLowerInvariant();
            var punaPutanja = Path.Combine(uploadFolder, fileName);
            slikaFajl.SaveAs(punaPutanja);

            return "/Content/Uploads/" + fileName;
        }
    }
}
