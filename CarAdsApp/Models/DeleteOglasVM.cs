namespace CarAdsApp.Models.ViewModels
{
    public class DeleteOglasVM
    {
        public string Id { get; set; } // ID oglasa
        public string Naziv { get; set; } // Naziv oglasa
        public string Marka { get; set; } // Marka automobila
        public string GodinaProizvodnje { get; set; } // Godina proizvodnje
        public string Cena { get; set; } // Cena automobila
        public string Opis { get; set; } // Opis oglasa
        public string KorisnikUsername { get; set; } // ID korisnika koji je postavio oglas
    }
}
