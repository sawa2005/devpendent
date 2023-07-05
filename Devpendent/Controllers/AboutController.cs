using Microsoft.AspNetCore.Mvc;
using SmartBreadcrumbs.Attributes;

namespace Devpendent.Controllers
{
    [Breadcrumb("About us")]
    public class AboutController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [Breadcrumb("ViewData.Title")]
        public IActionResult HowTo()
        {
            return View();
        }
    }
}
