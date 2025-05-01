using CarAdsApp.Models;
using CarAdsApp.Models.ViewModels;
using CarAdsApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace CarAdsApp.Controllers
{
    public class OglasController : Controller
    {
        private readonly OglasiServices _oglasiServices;
        private readonly UserServices _userServices;

        public OglasController(OglasiServices oglasiServices, UserServices userServices)
        {
            _oglasiServices = oglasiServices;
            _userServices = userServices;
        }

        [HttpGet]
        public IActionResult Index()
        {
            if (!HttpContext.Session.Keys.Contains("UserId"))
                return RedirectToAction("Index", "Home");

            var oglasi = _oglasiServices.GetAll();
            return View(oglasi);
        }

        [HttpGet]
        public IActionResult DodajOglas()
        {
            return View();
        }

        [HttpPost]
        public IActionResult DodajOglas(Oglas oglas)
        {
            ModelState.Remove(nameof(Oglas.Id));
            ModelState.Remove(nameof(Oglas.KorisnikId));

            if (!ModelState.IsValid)
                return View(oglas);

            oglas.KorisnikId = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(oglas.KorisnikId))
                return RedirectToAction("Index", "Home");

            oglas.Komentari = new List<Komentar>();
            _oglasiServices.Create(oglas);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Detalji(string id)
        {
            if (!HttpContext.Session.Keys.Contains("UserId"))
                return RedirectToAction("Index", "Home");

            var oglas = _oglasiServices.GetById(id);
            if (oglas == null)
                return NotFound();

            var korisnik = _userServices.GetById(oglas.KorisnikId);

            var viewModel = new OglasDetaljiVM
            {
                Id = oglas.Id,
                Naziv = oglas.Naziv,
                Marka = oglas.Marka,
                GodinaProizvodnje = oglas.GodinaProizvodnje,
                Cena = oglas.Cena,
                Opis = oglas.Opis,
                KorisnikId = oglas.KorisnikId,
                KorisnikUsername = korisnik?.Username ?? "Nepoznat Korisnik",
                Komentari = oglas.Komentari?.Select(k => new KomentarVM
                {
                    Tekst = k.Tekst,
                    KorisnikId = k.KorisnikId,
                    KorisnikUsername = k.KorisnikUsername,
                    OglasId = k.OglasId
                }).ToList() ?? new List<KomentarVM>()
            };

            return View(viewModel);
        }



        [HttpGet]
        public IActionResult Edit(string id)
        {
            var oglas = _oglasiServices.GetById(id);
            if (oglas == null)
                return NotFound();

            var vm = new EditOglasVM
            {
                Id = oglas.Id,
                Naziv = oglas.Naziv,
                Marka = oglas.Marka,
                GodinaProizvodnje = oglas.GodinaProizvodnje,
                Cena = oglas.Cena,
                Opis = oglas.Opis
            };

            return View(vm);
        }


        [HttpPost]
        public IActionResult Edit(EditOglasVM vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var oglas = _oglasiServices.GetById(vm.Id);
            if (oglas == null)
                return NotFound();

            oglas.Naziv = vm.Naziv;
            oglas.Marka = vm.Marka;
            oglas.GodinaProizvodnje = vm.GodinaProizvodnje;
            oglas.Cena = vm.Cena;
            oglas.Opis = vm.Opis;

            _oglasiServices.Update(vm.Id, oglas);
            return RedirectToAction("Index");
        }


        [HttpGet]
        public IActionResult Delete(string id)
        {
            if (!HttpContext.Session.Keys.Contains("UserId"))
                return RedirectToAction("Index", "Home");

            var oglas = _oglasiServices.GetById(id);
            if (oglas == null)
                return NotFound();

            var user = _userServices.GetById(oglas.KorisnikId);

            var vm = new DeleteOglasVM()
            {
                Id = oglas.Id,
                Naziv = oglas.Naziv,
                Marka = oglas.Marka,
                GodinaProizvodnje = oglas.GodinaProizvodnje,
                Cena = oglas.Cena,
                Opis = oglas.Opis,
                KorisnikUsername = user?.Username ?? "Nepoznat Korisnik"
            };

            return View(vm);
        }

        [HttpPost]
        public IActionResult DeleteConfirmed(string id)
        {
            var oglas = _oglasiServices.GetById(id);
            if (oglas == null)
                return NotFound();

            _oglasiServices.Delete(id);
            return RedirectToAction("Index");
        }


        [HttpPost]
        public IActionResult DodajKomentar(KomentarVM model)
        {
            ModelState.Remove(nameof(KomentarVM.KorisnikId));
            ModelState.Remove(nameof(KomentarVM.KorisnikUsername));

            if (!ModelState.IsValid)
                return RedirectToAction("Detalji", new { id = model.OglasId });
            var oglas = _oglasiServices.GetById(model.OglasId);
            if (oglas == null)
                return NotFound();
            var komentar = new Komentar
            {
                Id = Guid.NewGuid().ToString(),
                Tekst = model.Tekst,
                OglasId = model.OglasId,
                KorisnikId = HttpContext.Session.GetString("UserId"),
                KorisnikUsername = HttpContext.Session.GetString("Username")
            };
            if(oglas.Komentari == null)
            {
                oglas.Komentari = new List<Komentar>();
            }
            oglas.Komentari.Add(komentar);
            _oglasiServices.Update(model.OglasId, oglas);
            Console.WriteLine("Dodavanje komentara za oglas ID: " + model.OglasId);

            return RedirectToAction("Detalji", new { id = model.OglasId });
        }

        public IActionResult TestKomentar()
        {
            var oglas = _oglasiServices.GetAll().FirstOrDefault(); // uzmi prvi oglas

            if (oglas == null)
                return Content("Nema oglasa");

            if (oglas.Komentari == null)
                oglas.Komentari = new List<Komentar>();

            oglas.Komentari.Add(new Komentar
            {
                Id = Guid.NewGuid().ToString(),
                OglasId = oglas.Id,
                Tekst = "TEST KOMENTAR",
                KorisnikId = "1",
                KorisnikUsername = "Tester"
            });

            _oglasiServices.Update(oglas.Id, oglas);
            return Content("Test komentar dodat");
        }
    }
}