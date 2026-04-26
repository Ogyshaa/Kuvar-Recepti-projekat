using System.Collections.Generic;

namespace Kuvar.Models
{
    public class AdminReportViewModel
    {
        public int BrojKorisnika { get; set; }
        public int BrojRecepata { get; set; }
        public List<ReceptPoKategorijiViewModel> Kategorije { get; set; }
        public List<Recept> NajnovijiRecepti { get; set; }
    }

    public class ReceptPoKategorijiViewModel
    {
        public string NazivKategorije { get; set; }
        public int BrojRecepata { get; set; }
    }
}
