using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kuvar.Models
{
	public class ProfilePregledRecepta
	{
        public Korisnik PrijavljeniKorisnik { get; set; }
        public List<Recept> ObjavljeniRecipti { get; set; }
    }
}