using System;
using System.ComponentModel.DataAnnotations;

namespace Kuvar.Models
{
    public class ProfileEditViewModel
    {
        [Required]
        [Display(Name = "Korisnicko ime")]
        public string KorIme { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Datum rodjenja")]
        public DateTime DatumRodjenja { get; set; }
    }
}
