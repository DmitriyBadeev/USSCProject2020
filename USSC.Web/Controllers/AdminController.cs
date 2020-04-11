using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using USSC.Services;
using USSC.Services.PermissionServices;
using USSC.Services.UserServices.Interfaces;
using USSC.Web.ViewModels;

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

                var viewModels = users.Select(u => new UserViewModel()
                {
                    Id = u.Id,
                    Email = u.Email,
                    Name = $"{u.Name} {u.LastName}",
                    Roles = string.Join(", ", _userDataService.GetUserRoles(u.Id).Result),
                    Accesses = string.Join(", ", _accessManager.GetAccessibleSubsystems(u.Id).Result)
                });
                
                return View(viewModels);
            }

            return Forbid(CookieAuthenticationDefaults.AuthenticationScheme);
        }
    }
}