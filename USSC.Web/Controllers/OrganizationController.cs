using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using USSC.Services;
using USSC.Services.PermissionServices;
using USSC.Services.UserServices.Interfaces;

namespace USSC.Web.Controllers
{
    public class OrganizationController : Controller
    {
        private readonly IAccessManager _accessManager;
        private readonly IUserDataService _userData;
        private readonly string _organizationSubsystemName;

        public OrganizationController(IAccessManager accessManager, IUserDataService userData)
        {
            _accessManager = accessManager;
            _userData = userData;
            _organizationSubsystemName = Constants.OrganizationSubsystem;
        }

        public async Task<IActionResult> Main()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            var userData = await _userData.GetUserData(User.Identity.Name);
            var isAdmin = await _accessManager.IsAdmin(userData.Id);
            
            if (isAdmin)
            {
                return RedirectToAction("Index", "Admin");
            }

            return RedirectToAction("Index", "Organization");
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var hasPermission = await _accessManager.HasPermission(User.Identity.Name, _organizationSubsystemName);

            if (hasPermission)
            {
                return View();
            }

            return Forbid(CookieAuthenticationDefaults.AuthenticationScheme);
        }
    }
}