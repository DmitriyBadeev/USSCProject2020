using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace USSC.Web.Controllers
{
    public class PersonalController : Controller
    {
        [Authorize]
        public IActionResult Index()
        {
            return View();
        }
    }
}