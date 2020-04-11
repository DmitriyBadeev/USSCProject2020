using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using USSC.Services;
using USSC.Services.PermissionServices;
using USSC.Services.UserServices.Interfaces;

namespace USSC.Web.Controllers
{
    public class AdminController : Controller
    {
        private readonly IAccessManager _accessManager;
        private readonly IUserDataService _userDataService;
        private readonly string _adminSubsystemName;

        public AdminController(IAccessManager accessManager, IUserDataService userDataService)
        {
            _accessManager = accessManager;
            _userDataService = userDataService;
            _adminSubsystemName = Constants.AdminSubsystem;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var hasPermission = await _accessManager.HasPermission(User.Identity.Name, _adminSubsystemName);

            if (hasPermission)
            {
                var users = _userDataService.GetAllUsers();
                return View(users);
            }

            return Forbid(CookieAuthenticationDefaults.AuthenticationScheme);
        }
    }
}