using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Kuvar.Models
{
	public class Korisnik
	{
        public int Id { get; set; }

        [Required]
        [Display(Name = "Korisnicko ime")]
        public string KorIme { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Datum rodjenja")]
        public DateTime DatumRodjenja { get; set; }


        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 3)]
        public string Lozinka { get; set; }
    }
}
