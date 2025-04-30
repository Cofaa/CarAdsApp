using System.ComponentModel.DataAnnotations;

namespace CarAdsApp.Models.ViewModels
{
    public class EditOglasVM
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "Naziv je obavezan.")]
        public string Naziv { get; set; }

        [Required(ErrorMessage = "Marka je obavezna.")]
        public string Marka { get; set; }

        [Required(ErrorMessage = "Godina proizvodnje je obavezna.")]
        public string GodinaProizvodnje { get; set; }

        [Required(ErrorMessage = "Cena je obavezna.")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Cena mora biti broj.")]
        public string Cena { get; set; }

        [Required(ErrorMessage = "Opis je obavezan.")]
        public string Opis { get; set; }
    }
}