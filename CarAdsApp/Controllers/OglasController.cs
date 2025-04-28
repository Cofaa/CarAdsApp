using CarAdsApp.Models;
using CarAdsApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;

namespace CarAdsApp.Controllers
{
    public class OglasController : Controller
    {
        private readonly OglasiServices _oglasiServices; // Servis za oglase
      //  private readonly UserServices _userServices; // Servis za korisnike

        public OglasController(OglasiServices oglasiServices, UserServices userServices) // Konstruktori
        {
            _oglasiServices = oglasiServices; // Inicijalizacija servisa
        //    _userServices = userServices; // Inicijalizacija servisa
        }

        //public IActionResult TestInsert()
        //{
        //    var testOglas = new Oglas
        //    {
        //        Naziv = "Test auto",
        //        Marka = "Test marka",
        //        GodinaProizvodnje = "2022",
        //        Cena = "9999",
        //        Opis = "Test oglas - samo za proveru"
        //    };

        //    _oglasiServices.Create(testOglas);

        //    return Content("Test oglas ubačen (ako je sve OK)");
        //}



        [HttpGet]
        public IActionResult DodajOglas() // Akcija za prikaz svih oglasa
        {
            return View(); // Vraća pogled za dodavanje oglasa
        }

        [HttpPost]
        public IActionResult DodajOglas(Oglas oglas)
        {
            ModelState.Remove(nameof(Oglas.Id));
            if (!ModelState.IsValid)
            {
                return View(oglas); // Vrati view sa validacionim greškama
            }

            _oglasiServices.Create(oglas); // Insert u MongoDB
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Index()
        {
            if(!HttpContext.Session.Keys.Contains("UserId")) // Proverava da li je korisnik prijavljen
            {
                return RedirectToAction("Index", "Home"); // Preusmerava na početnu stranicu ako korisnik nije prijavljen
            }
            var oglasi = _oglasiServices.GetAll(); // Prikaz svih oglasa
            return View(oglasi); // Vraća pogled sa svim oglasima
        }

    }
}
