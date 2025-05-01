using System.ComponentModel.DataAnnotations;

namespace CarAdsApp.Models
{
    public class KomentarVM
    {
        [Required(ErrorMessage = "Tekst komentara je obavezan.")]
        public string Tekst { get; set; } // Tekst komentara
        public string OglasId { get; set; } // ID oglasa na koji se komentar odnosi
        public string KorisnikId { get; set; } // ID korisnika koji je napisao komentar
        //public string KorisnikIme { get; set; } // Ime korisnika koji je napisao komentar

        public string KorisnikUsername { get; set; } // Korisničko ime korisnika koji je napisao komentar
    }
}
