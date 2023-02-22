using Microsoft.AspNetCore.Mvc;
using Frisk_2._0.Models;

namespace Frisk_2._0.Controllers
{
    public class LoggedInController : Controller
    {
        public IActionResult Index(int userId)
        {
            // Hämta användar-id från sessionen
           

            // Skicka användarens ID till vyn
            var userData = new UserData { Id = userId };
            return View(userData);
        }
    }
}
