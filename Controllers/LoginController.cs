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

        public async Task<IActionResult> LogIn(LogIn logIn)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Sätt länken till API:et
                    client.BaseAddress = new Uri("http://193.10.202.75/FriskAPI/Users");

                    // Skapa en GET-begäran med användardata
                    var response = await client.GetAsync($"Users?email={logIn.Email}&password={logIn.Password}");

                    // Kontrollera om begäran lyckades
                    if (response.IsSuccessStatusCode)
                    {
                        // Läs svar från API:et och omvandla det till en lista med användarobjekt
                        var users = JsonConvert.DeserializeObject<List<LogIn>>(await response.Content.ReadAsStringAsync());

                        // Hitta användaren med rätt e-postadress och lösenord
                        var user = users.Find(x => x.Email == logIn.Email && x.Password == logIn.Password);

                        // Kontrollera om användaren existerar och lösenordet är korrekt
                        if (user != null)
                        {
                            // Logga in användaren
                            // ...

                            // Returnera en inloggad vy till användaren
                            return RedirectToAction("Index", "LoggedIn");
                        }

                    }
                }
                catch (Exception ex)
                {

                    ViewBag.ErrorMessage = "Fel användarnamn eller lösenord";
                    return View();

                }
                ViewBag.ErrorMessage = "Fel användarnamn eller lösenord";
                return View();
            }
            
        }
    }
}
