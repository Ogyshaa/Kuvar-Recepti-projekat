using Kuvar.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace Kuvar.Service
{
    public class KorisniciService
    {
        private string PathToFile
        {
            get { return HttpContext.Current.Server.MapPath("~/App_Data/korisnici.xml"); }
        }

        public List<Korisnik> GetAll()
        {
            var doc = XDocument.Load(PathToFile);

            return doc.Descendants("Korisnik")
                .Select(u => new Korisnik
                {
                    Id = (int)u.Element("Id"),
                    KorIme = (string)u.Element("KorIme"),
                    DatumRodjenja = (DateTime?)u.Element("DatumRodjenja") ?? new DateTime(2000, 1, 1),
                    Email = (string)u.Element("Email") ?? string.Empty,
                    Lozinka = (string)u.Element("Lozinka") ?? string.Empty
                })
                .ToList();
        }

        public void Register(Korisnik user)
        {
            var users = GetAll();

            if (users.Any(x => x.KorIme.Equals(user.KorIme, StringComparison.OrdinalIgnoreCase)))
            {
                throw new InvalidOperationException("Korisnicko ime je vec zauzeto.");
            }

            if (users.Any(x => !string.IsNullOrWhiteSpace(x.Email) && x.Email.Equals(user.Email, StringComparison.OrdinalIgnoreCase)))
            {
                throw new InvalidOperationException("Email je vec zauzet.");
            }

            var doc = XDocument.Load(PathToFile);

            var newId = doc.Descendants("Korisnik").Any()
                ? doc.Descendants("Korisnik").Max(u => (int)u.Element("Id")) + 1
                : 1;

            user.Id = newId;

            doc.Root.Add(new XElement("Korisnik",
                new XElement("Id", user.Id),
                new XElement("KorIme", user.KorIme),
                new XElement("DatumRodjenja", user.DatumRodjenja.ToString("yyyy-MM-dd")),
                new XElement("Email", user.Email),
                new XElement("Lozinka", user.Lozinka)
            ));

            doc.Save(PathToFile);
        }

        public Korisnik Login(string korime, string loz)
        {
            return GetAll().FirstOrDefault(u => u.KorIme == korime && u.Lozinka == loz);
        }

        public void Update(Korisnik korisnik)
        {
            var others = GetAll().Where(x => x.Id != korisnik.Id).ToList();

            if (others.Any(x => x.KorIme.Equals(korisnik.KorIme, StringComparison.OrdinalIgnoreCase)))
            {
                throw new InvalidOperationException("Korisnicko ime je vec zauzeto.");
            }

            if (others.Any(x => !string.IsNullOrWhiteSpace(x.Email) && x.Email.Equals(korisnik.Email, StringComparison.OrdinalIgnoreCase)))
            {
                throw new InvalidOperationException("Email je vec zauzet.");
            }

            var doc = XDocument.Load(PathToFile);
            var existing = doc.Descendants("Korisnik").FirstOrDefault(x => (int)x.Element("Id") == korisnik.Id);

            if (existing == null)
            {
                return;
            }

            existing.SetElementValue("KorIme", korisnik.KorIme);
            existing.SetElementValue("DatumRodjenja", korisnik.DatumRodjenja.ToString("yyyy-MM-dd"));
            existing.SetElementValue("Email", korisnik.Email);
            existing.SetElementValue("Lozinka", korisnik.Lozinka);

            doc.Save(PathToFile);
        }

        public void Delete(int id)
        {
            var doc = XDocument.Load(PathToFile);
            var existing = doc.Descendants("Korisnik").FirstOrDefault(x => (int)x.Element("Id") == id);

            if (existing == null)
            {
                return;
            }

            existing.Remove();
            doc.Save(PathToFile);
        }
    }
}
