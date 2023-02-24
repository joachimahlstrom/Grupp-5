//Byt till ert projektnamn.Models
using Frisk_2._0.Models;
using Microsoft.AspNetCore.Mvc;

//Byt till ert projektnamn.Controllers
namespace Frisk_2._0.Controllers
{
    public class LoggedInController : Controller
    {
        //Använd er controller där ni vill att användarinfon ska visas upp
        public IActionResult Index(UserData userData)
        {
            // Kontrollera att användardata har skickats med och att det finns ett giltigt användar-id
            if (userData != null && userData.Id > 0)
            {
                // Returnera vyn med användaruppgifterna
                return View(userData);
            }

            // Om det inte finns giltig användarinformation, skicka användaren tillbaka till Login-sidan med ett felmeddelande
            TempData["Message"] = "Försök logga in igen";
            return RedirectToAction("Index", "Login");
        }
    }
}
