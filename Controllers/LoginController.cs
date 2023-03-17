using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;
using Frisk_2._0.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

namespace Frisk_2._0.Controllers
{
    public class LoginController : Controller
    {
        private readonly HttpClient _httpClient;

        public LoginController()
        {
            // Skapa en HTTP-klient
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("http://193.10.202.75/FriskAPI/");
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LogIn logIn)
        {
            // Skapa en GET-begäran med användardata
            var response = await _httpClient.GetAsync($"Users?email={logIn.Email}&password={logIn.Lösenord}");

            // Kontrollera om begäran lyckades
            if (response.IsSuccessStatusCode)
            {
                // Läs svar från API:et och omvandla det till en lista med användarobjekt
                var users = JsonConvert.DeserializeObject<List<UserData>>(await response.Content.ReadAsStringAsync());

                // Hitta användaren med rätt e-postadress och lösenord
                var user = users.Find(x => x.Email == logIn.Email && x.Password == logIn.Lösenord);

                // Kontrollera om användaren existerar och lösenordet är korrekt
                if (user != null)
                {
                    // Skapa en ny UserData-modell och spara användaruppgifterna
                    var userData = new UserData
                    {
                        Id = user.Id,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Email = user.Email,
                        Password = user.Password,
                        UserType = user.UserType
                    };

                    // Spara användaruppgifterna i en session
                    HttpContext.Session.SetString("UserData", JsonConvert.SerializeObject(userData));

                    // Skapa en claims identity med användarens id
                    var identity = new ClaimsIdentity(new[] { new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()) }, CookieAuthenticationDefaults.AuthenticationScheme);

                    // Skapa en autentiseringscookie
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

                    // Skicka användarens uppgifter till LoggedIn-vyn samt kolla om användaren är admin
                    if (userData.UserType == "admin")
                    {
                        return RedirectToAction("Index", "Admin");
                    }
                    else
                    {
                        return RedirectToAction("Index", "LoggedIn", userData);
                    }
                    
                }
            }

            // Om användarnamn eller lösenord är felaktiga, skicka tillbaka till login-sidan med ett felmeddelande
            TempData["Message"] = "Felaktigt användarnamn eller lösenord";
            return RedirectToAction("Index");
        }
        //Funktion som loggar ut användaren och omdirigerar tillbaka till inloggningssidan.
        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(
            CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Index", "Login");
        }
    }
}
