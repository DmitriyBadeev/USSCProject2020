using Microsoft.AspNetCore.Mvc;

namespace USSC.Web.Controllers
{
    public class CommonController : Controller
    {
        public IActionResult Denied()
        {
            return View("Denied");
        }

        public IActionResult Error()
        {
            return View("Error");
        }
    }
}