namespace CarAdsApp.Models
{
    public class OglasDetaljiVM
    {
        public string Id { get; set; } // ID oglasa
        public string Naziv { get; set; } // Naziv oglasa
        public string Marka { get; set; } // Marka automobila
        public string GodinaProizvodnje { get; set; } // Godina proizvodnje
        public string Cena { get; set; } // Cena automobila
        public string Opis { get; set; } // Opis oglasa
        public string KorisnikId { get; set; } // ID korisnika koji je postavio oglas
        public string KorisnikUsername { get; set; } // Korisničko ime korisnika koji je postavio oglas
        public List<KomentarVM> Komentari { get; set; } = new(); // Lista komentara vezanih za oglas
    }
}
