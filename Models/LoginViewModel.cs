using System.ComponentModel.DataAnnotations;

namespace Kuvar.Models
{
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Korisnicko ime")]
        public string KorIme { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Lozinka")]
        public string Lozinka { get; set; }
    }
}
