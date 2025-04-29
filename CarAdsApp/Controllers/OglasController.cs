using AspNetCoreGeneratedDocument;
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
        private readonly UserServices _userServices; // Servis za korisnike

        public OglasController(OglasiServices oglasiServices, UserServices userServices) // Konstruktori
        {
            _oglasiServices = oglasiServices; // Inicijalizacija servisa
            _userServices = userServices; // Inicijalizacija servisa
        }

        [HttpGet]
        public IActionResult DodajOglas() // Akcija za prikaz svih oglasa
        {
            return View(); // Vraća pogled za dodavanje oglasa
        }

        [HttpPost]
        public IActionResult DodajOglas(Oglas oglas)
        {
            ModelState.Remove(nameof(Oglas.Id));
            ModelState.Remove(nameof(Oglas.KorisnikId));
            if (!ModelState.IsValid)
            {
                return View(oglas); // Vrati view sa validacionim greškama
            }

            oglas.KorisnikId = HttpContext.Session.GetString("UserId"); // Dobija ID korisnika iz sesije
            if (string.IsNullOrEmpty(oglas.KorisnikId)) // Proverava da li je korisnik prijavljen
            {
                return RedirectToAction("Index", "Home"); // Preusmerava na početnu stranicu ako korisnik nije prijavljen
            }
            _oglasiServices.Create(oglas); // Insert u MongoDB
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Index()
        {
            if (!HttpContext.Session.Keys.Contains("UserId")) // Proverava da li je korisnik prijavljen
            {
                return RedirectToAction("Index", "Home"); // Preusmerava na početnu stranicu ako korisnik nije prijavljen
            }
            var oglasi = _oglasiServices.GetAll(); // Prikaz svih oglasa
            return View(oglasi); // Vraća pogled sa svim oglasima
        }

        public IActionResult Detalji(string id)
        {
            if (!HttpContext.Session.Keys.Contains("UserId")) // Proverava da li je korisnik prijavljen
            {
                return RedirectToAction("Index", "Home"); // Preusmerava na početnu stranicu ako korisnik nije prijavljen
            }

            var oglas = _oglasiServices.GetById(id); // Prikaz detalja oglasa
            if (oglas == null) // Proverava da li je oglas pronađen
            {
                return NotFound(); // Vraća 404 ako oglas nije pronađen
            }

            var userId = HttpContext.Session.GetString("UserId"); // Dobija ID korisnika iz sesije
            var user = _userServices.GetById(oglas.KorisnikId); // Pronalazi korisnika po ID-u

            var viewModel = new OglasDetaljiVM
            {
                Id = oglas.Id,
                Naziv = oglas.Naziv,
                Marka = oglas.Marka,
                GodinaProizvodnje = oglas.GodinaProizvodnje,
                Cena = oglas.Cena,
                Opis = oglas.Opis,
                KorisnikId = oglas.KorisnikId,
                KorisnikUsername = user?.Username ?? "Nepoznat Korisnik"// Prikaz korisničkog imena
            };

            return View(viewModel); // Vraća pogled sa detaljima oglasa
        }
    }
}
//                KorisnikUsername = user?.Username ?? // Prikaz korisničkog imena
//}
