using Microsoft.AspNetCore.Mvc;

namespace Frisk_2._0.Controllers
{
    public class ServiceController : Controller
    {
        public IActionResult Index()
        {
            return RedirectToAction("Index", "Login");
        }
        public IActionResult Grupp1()
        {
            return Redirect("http://193.10.202.75/Frisk2.0/LogIn");
        }
        public IActionResult Grupp2()
        {
            return Redirect("https://informatik2.ei.hv.se/myevents/Login");
        }
        public IActionResult Grupp3()
        {
            return Redirect("https://informatik3.ei.hv.se/frisk");
        }
        public IActionResult Grupp4()
        {
            return Redirect("https://informatik4.ei.hv.se/SponsorOffer/Login/Index");
        }
        public IActionResult Grupp6()
        {
            return Redirect("http://193.10.202.75/Frisk2.0/LogIn");
        }
    }
}
