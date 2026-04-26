using System.Collections.Generic;

namespace Kuvar.Models
{
    public class ReceptIndexViewModel
    {
        public List<Recept> Recepti { get; set; }
        public string Pretraga { get; set; }
        public string Kategorija { get; set; }
        public string Sortiranje { get; set; }
        public List<string> Kategorije { get; set; }
    }
}
