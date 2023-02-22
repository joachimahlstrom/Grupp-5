using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;
using Frisk_2._0.Models;

namespace Frisk_2._0.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Login(LogIn logIn)
        {
            // Skapa en HTTP-klient
            using var client = new HttpClient();

            // Sätt länken till API:et
            client.BaseAddress = new Uri("http://193.10.202.75/FriskAPI/Users");

            // Skapa en GET-begäran med användardata
            var response = await client.GetAsync($"Users?email={logIn.Email}&password={logIn.Password}");

            // Kontrollera om begäran lyckades
            if (response.IsSuccessStatusCode)
            {
                // Läs svar från API:et och omvandla det till en lista med användarobjekt
                var users = JsonConvert.DeserializeObject<List<UserData>>(await response.Content.ReadAsStringAsync());

                // Hitta användaren med rätt e-postadress och lösenord
                var user = users.Find(x => x.Email == logIn.Email && x.Password == logIn.Password);

                // Kontrollera om användaren existerar och lösenordet är korrekt
                if (user != null)
                {
                    

                   

                    // Skicka användarens ID till LoggedIn-vyn
                    return RedirectToAction("Index", "LoggedIn", new { userId = user.Id });
                }
            }

            // Om användarnamn eller lösenord är felaktiga, skicka tillbaka till login-sidan med ett felmeddelande
            TempData["Message"] = "Felaktigt användarnamn eller lösenord";
            return RedirectToAction("Index");
        }
    }
}