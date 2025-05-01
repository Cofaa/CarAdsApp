using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;


namespace CarAdsApp.Models
{
    public class Komentar
    {
        public string Id { get; set; } = Guid.NewGuid().ToString(); // ID komentara
        public string OglasId { get; set; } // ID oglasa na koji se komentar odnosi
        public string KorisnikId { get; set; } // ID korisnika koji je napisao komentar

        public string KorisnikUsername { get; set; } // Ime korisnika koji je napisao komentar

        [Required]
        public string Tekst { get; set; } // Tekst komentara

    }
}
