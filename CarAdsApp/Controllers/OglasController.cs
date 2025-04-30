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

            var user = _userServices.GetById(oglas.KorisnikId);
            var viewModel = new OglasDetaljiVM
            {
                Id = oglas.Id,
                Naziv = oglas.Naziv,
                Marka = oglas.Marka,
                GodinaProizvodnje = oglas.GodinaProizvodnje,
                Cena = oglas.Cena,
                Opis = oglas.Opis,
                KorisnikId = oglas.KorisnikId,
                KorisnikUsername = user?.Username ?? "Nepoznat Korisnik"
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
    }
}