using CarAdsApp.Models;
using CarAdsApp.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;

namespace CarAdsApp.Controllers
{
    public class UserController : Controller
    {
        private readonly UserServices _userServices; // Servis za korisnike

        public UserController(UserServices userServices) // Konstruktori
        {
            _userServices = userServices; // Inicijalizacija servisa
        }

        [HttpGet]
        public IActionResult Login() => View(new LoginViewModel()); // Akcija za prijavu

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginViewModel vm)
        {
            if(!ModelState.IsValid) // Proveri da li je model validan
            {
                return View(vm); // Vraća pogled sa greškom
            }
            var existingUser = _userServices.GetByUsername(vm.Username); // Proveri da li korisnik postoji
            if(existingUser == null) // Ako ne postoji
            {
                ModelState.AddModelError("Username", "Korisnik ne postoji");
                return View(vm); // Vraća pogled sa greškom
            }

            if (existingUser.PasswordHash != HashPassowrd(vm.Password)) // Proveri lozinku
            {
                ModelState.AddModelError("Password", "Pogresna lozinka");
                return View(vm); // Vraća pogled sa greškom
            }

            HttpContext.Session.SetString("UserId", existingUser.Id); // Postavi sesiju 
            
            HttpContext.Session.SetString("Username", existingUser.Username);

            return RedirectToAction("Index", "Home"); // Preusmeravanje na početnu stranicu
        }

        // Logut

        [HttpPost]
        public IActionResult Logout() // Akcija za odjavu
        {
            

            HttpContext.Session.Remove("Username"); // Ukloni sesiju
            HttpContext.Session.Remove("UserId");
            return RedirectToAction("Login"); // Preusmeravanje na početnu stranicu
        }

        

        [HttpGet]
        public IActionResult Register() // Akcija za registraciju
        {
            return View(); // Vraća pogled za registraciju
        }

        [HttpPost]
        public IActionResult Register(User user)
        {
            if (_userServices.GetByUsername(user.Username) != null) // Proveri da li korisnik vec postoji
            {
                ModelState.AddModelError("Username", "Korisnicko ime vec postoji");
                return View(); // Vraća pogled sa greškom
            }

            user.PasswordHash = HashPassowrd(user.PasswordHash); // Hashiranje lozinke
            _userServices.Create(user); // Kreiranje korisnika
            return RedirectToAction("Login"); // Preusmeravanje na stranicu za prijavu
        }

        public string HashPassowrd(string password)
        {
            using var sha256 = SHA256.Create(); // Kreiranje SHA256 heš funkcije
            var bytes = Encoding.UTF8.GetBytes(password); // Konvertovanje lozinke u bajtove
            var hash = sha256.ComputeHash(bytes); // Heširanje lozinke
            return Convert.ToBase64String(hash); // Konvertovanje heša u string
        }
    }
}
