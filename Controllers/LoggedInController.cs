using Microsoft.AspNetCore.Mvc;

namespace Frisk_2._0.Controllers
{
    public class LoggedInController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
