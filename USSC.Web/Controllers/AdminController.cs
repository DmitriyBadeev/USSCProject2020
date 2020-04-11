using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using USSC.Services;
using USSC.Services.PermissionServices;

namespace USSC.Web.Controllers
{
    public class AdminController : Controller
    {
        private readonly IAccessManager _accessManager;
        private readonly string _adminSubsystemName;

        public AdminController(IAccessManager accessManager)
        {
            _accessManager = accessManager;
            _adminSubsystemName = Constants.AdminSubsystem;
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