using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using USSC.Infrastructure.Models;
using USSC.Services;
using USSC.Services.OrganizationServices;
using USSC.Services.PermissionServices;
using USSC.Services.UserServices.Interfaces;
using USSC.Web.ViewModels;
using USSC.Web.ViewModels.Account;
using USSC.Web.ViewModels.Organization;

namespace USSC.Web.Controllers
{
    public class OrganizationController : Controller
    {
        private readonly IAccessManager _accessManager;
        private readonly IUserDataService _userData;
        private readonly IOrganizationService _organizationService;
        private readonly string _organizationSubsystemName;

        public OrganizationController(IAccessManager accessManager, IUserDataService userData, IOrganizationService organizationService)
        {
            _accessManager = accessManager;
            _userData = userData;
            _organizationService = organizationService;
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

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var hasPermission = await _accessManager.HasPermission(User.Identity.Name, _organizationSubsystemName);

            if (hasPermission)
            {
                var userData = await _userData.GetUserData(User.Identity.Name);
                var isAdmin = await _accessManager.IsAdmin(userData.Id);

                if (!isAdmin)
                {
                    return RedirectToAction("Details", "Organization");
                }

                return View();
            }

            return Forbid(CookieAuthenticationDefaults.AuthenticationScheme);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Details()
        {
            var hasPermission = await _accessManager.HasPermission(User.Identity.Name, _organizationSubsystemName);

            if (hasPermission)
            {
                return View();
            }

            return Forbid(CookieAuthenticationDefaults.AuthenticationScheme);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> AddOrganization()
        {
            var users = _userData.GetAllUsers().Select(u => new Select()
            {
                Id = u.Id,
                Name = $"{u.Name} {u.LastName} {u.Patronymic}"
            });

            var model = new PostOrganizationViewModel()
            {
                Users = users
            };

            return View(model);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddOrganization(PostOrganizationViewModel model)
        {
            var organization = new Organization()
            {
                Name = model.Name,
                INN = model.INN,
                OGRN = model.OGRN
            };

            if (model.SelectedUserId != null)
            {
                var selectedUserId = (int) model.SelectedUserId;
                organization.UserId = selectedUserId;

                var user = await _userData.GetUserData(selectedUserId);
                organization.User = user;
            }

            _organizationService.Add(organization);

            return RedirectToAction("Index", "Admin");
        }
    }
}