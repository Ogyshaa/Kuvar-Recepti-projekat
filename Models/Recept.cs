using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Kuvar.Models
{
	public class Recept
	{
        public int Id { get; set; }

        [Required]
        [Display(Name = "Naziv recepta")]
        public string Naziv { get; set; }

        [Required]
        [Display(Name = "Kategorija")]
        public string Kategorija { get; set; }

        [Required]
        [Display(Name = "Sastojci")]
        public string Sastojci { get; set; }

        [Required]
        [Display(Name = "Uputstvo")]
        public string Uputstva { get; set; }

        [Display(Name = "Slika")]
        public string Slika { get; set; }

        public string Autor { get; set; }

        [Display(Name = "Datum objave")]
        public DateTime Objavljenj { get; set; }

        public List<string> Omiljeno { get; set; } = new List<string>();
    }
}
