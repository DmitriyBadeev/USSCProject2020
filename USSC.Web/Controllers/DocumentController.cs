using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using USSC.Services;
using USSC.Services.PermissionServices;

namespace USSC.Web.Controllers
{
    public class DocumentController : Controller
    {
        private readonly IAccessManager _accessManager;
        private readonly string _adminSubsystemName;

        public DocumentController(IAccessManager accessManager)
        {
            _accessManager = accessManager;
            _adminSubsystemName = Constants.DocumentSubsystem;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var hasPermission = await _accessManager.HasPermission(User.Identity.Name, _adminSubsystemName);

            if (hasPermission)
            {
                return View();
            }

            return Forbid(CookieAuthenticationDefaults.AuthenticationScheme);
        }
    }
}