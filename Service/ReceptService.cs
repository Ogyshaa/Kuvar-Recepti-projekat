using Kuvar.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace Kuvar.Service
{
    public class ReceptService
    {
        private string PathToFile
        {
            get { return HttpContext.Current.Server.MapPath("~/App_Data/recepti.xml"); }
        }

        public List<Recept> GetAll()
        {
            var doc = XDocument.Load(PathToFile);

            return doc.Descendants("Recept")
                .Select(r => new Recept
                {
                    Id = (int)r.Element("Id"),
                    Naziv = (string)r.Element("Naziv"),
                    Kategorija = (string)r.Element("Kategorija"),
                    Sastojci = (string)r.Element("Sastojci"),
                    Uputstva = (string)r.Element("Uputstva"),
                    Slika = (string)r.Element("Slika"),
                    Autor = (string)r.Element("Autor"),
                    Objavljenj = (DateTime?)r.Element("Objavljenj") ?? DateTime.Now,
                    Omiljeno = r.Element("Omiljeno") == null
                        ? new List<string>()
                        : r.Element("Omiljeno").Elements("Korisnik").Select(x => x.Value).ToList()
                })
                .ToList();
        }

        public Recept GetById(int id)
        {
            return GetAll().FirstOrDefault(x => x.Id == id);
        }

        public void Add(Recept recipe)
        {
            var doc = XDocument.Load(PathToFile);

            var newId = doc.Descendants("Recept").Any()
                ? doc.Descendants("Recept").Max(r => (int)r.Element("Id")) + 1
                : 1;

            recipe.Id = newId;
            recipe.Objavljenj = DateTime.Now;

            doc.Root.Add(new XElement("Recept",
                new XElement("Id", recipe.Id),
                new XElement("Naziv", recipe.Naziv),
                new XElement("Kategorija", recipe.Kategorija),
                new XElement("Sastojci", recipe.Sastojci),
                new XElement("Uputstva", recipe.Uputstva),
                new XElement("Slika", recipe.Slika ?? string.Empty),
                new XElement("Autor", recipe.Autor),
                new XElement("Objavljenj", recipe.Objavljenj.ToString("yyyy-MM-dd HH:mm:ss")),
                new XElement("Omiljeno")
            ));

            doc.Save(PathToFile);
        }

        public void Update(Recept recept)
        {
            var doc = XDocument.Load(PathToFile);
            var existing = doc.Descendants("Recept").FirstOrDefault(x => (int)x.Element("Id") == recept.Id);

            if (existing == null)
            {
                return;
            }

            existing.SetElementValue("Naziv", recept.Naziv);
            existing.SetElementValue("Kategorija", recept.Kategorija);
            existing.SetElementValue("Sastojci", recept.Sastojci);
            existing.SetElementValue("Uputstva", recept.Uputstva);
            existing.SetElementValue("Slika", recept.Slika ?? string.Empty);

            doc.Save(PathToFile);
        }

        public void Delete(int id)
        {
            var doc = XDocument.Load(PathToFile);
            var recipe = doc.Descendants("Recept").FirstOrDefault(x => (int)x.Element("Id") == id);
            recipe?.Remove();
            doc.Save(PathToFile);
        }

        public void DeleteByAuthor(string username)
        {
            var doc = XDocument.Load(PathToFile);
            var recepti = doc.Descendants("Recept")
                .Where(x => (string)x.Element("Autor") == username)
                .ToList();

            foreach (var recept in recepti)
            {
                recept.Remove();
            }

            doc.Save(PathToFile);
        }

        public void ToggleFavorite(int id, string username)
        {
            var doc = XDocument.Load(PathToFile);
            var recipe = doc.Descendants("Recept").FirstOrDefault(x => (int)x.Element("Id") == id);

            if (recipe == null)
            {
                return;
            }

            var favorites = recipe.Element("Omiljeno");
            if (favorites == null)
            {
                favorites = new XElement("Omiljeno");
                recipe.Add(favorites);
            }

            var existing = favorites.Elements("Korisnik").FirstOrDefault(x => x.Value == username);

            if (existing != null)
            {
                existing.Remove();
            }
            else
            {
                favorites.Add(new XElement("Korisnik", username));
            }

            doc.Save(PathToFile);
        }

        public void UpdateUsernameReferences(string oldUsername, string newUsername)
        {
            var doc = XDocument.Load(PathToFile);

            foreach (var recipe in doc.Descendants("Recept"))
            {
                var autor = recipe.Element("Autor");
                if (autor != null && autor.Value == oldUsername)
                {
                    autor.Value = newUsername;
                }

                var favorites = recipe.Element("Omiljeno");
                if (favorites == null)
                {
                    continue;
                }

                foreach (var korisnik in favorites.Elements("Korisnik").Where(x => x.Value == oldUsername))
                {
                    korisnik.Value = newUsername;
                }
            }

            doc.Save(PathToFile);
        }

        public string ExportAsXml()
        {
            return File.ReadAllText(PathToFile);
        }
    }
}
