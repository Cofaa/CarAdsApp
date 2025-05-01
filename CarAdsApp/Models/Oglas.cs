using Microsoft.AspNetCore.Mvc.ModelBinding;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace CarAdsApp.Models
{
    public class Oglas
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [BindNever] // ⬅⬅⬅⬅⬅⬅⬅⬅⬅⬅⬅
        public string Id { get; set; }


        public List<Komentar> Komentari { get; set; } = new List<Komentar>(); // Lista komentara vezanih za oglas

        [BindNever] //
        public string KorisnikId { get; set; } // ID korisnika koji je postavio oglas


        [Required(ErrorMessage = "Naziv je obavezan.")]
        public string Naziv { get; set; }

        [Required(ErrorMessage = "Marka je obavezna.")]
        public string Marka { get; set; }

        [Required(ErrorMessage = "Godina proizvodnje je obavezna.")]
        [Display(Name = "Godina proizvodnje")]
        public string GodinaProizvodnje { get; set; }

        [Required(ErrorMessage = "Cena je obavezna.")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Cena mora biti broj.")]
        public string Cena { get; set; }

        [Required(ErrorMessage = "Opis je obavezan.")]
        public string Opis { get; set; }


    }
}
