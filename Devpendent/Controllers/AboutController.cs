using Microsoft.AspNetCore.Mvc;

namespace Devpendent.Controllers
{
    public class AboutController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult HowTo()
        {
            return View();
        }
    }
}
