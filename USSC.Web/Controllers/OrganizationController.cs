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
using USSC.Web.ViewModels.Employee;
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
                var organizations = _organizationService.GetAll(); 

                if (!isAdmin)
                {
                    var id = userData.Organization.Id;
                    return RedirectToAction("Details", "Organization", new { organizationId = id });
                }

                var organizationViewModel = organizations.Select(o =>
                {
                    string userName = null;
                    string phone = null;
                    if (o.User != null)
                    {
                        userName = $"{o.User.LastName} {o.User.Name} {o.User.Patronymic}";
                        phone = o.User.Phone;
                    }

                    return new OrganizationTableViewModel()
                    {
                        Id = o.Id,
                        Name = o.Name,
                        UserName = userName,
                        UserPhone = phone
                    };
                });

                return View(organizationViewModel);
            }

            return Forbid(CookieAuthenticationDefaults.AuthenticationScheme);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Details(int organizationId)
        {
            var hasPermission = await _accessManager.HasPermission(User.Identity.Name, _organizationSubsystemName);

            if (hasPermission)
            {
                var organization = _organizationService.GetById(organizationId);
                var user = organization.User;

                var userName = $"{user.LastName} {user.Name} {user.Patronymic}";

                var employees = organization.Employees
                    .Select(e => new EmployeeTableViewModel()
                    {
                        Id = e.Id,
                        Name = e.Name,
                        Phone = e.Phone,
                        Position = e.Position.Name
                    });

                var model = new OrganizationViewModel()
                {
                    Id = organizationId,
                    Name = organization.Name,
                    INN = organization.INN,
                    OGRN = organization.OGRN,
                    UserName = userName,
                    Email = user.Email,
                    Phone = user.Phone,
                    Employees = employees
                };

                return View(model);
            }

            return Forbid(CookieAuthenticationDefaults.AuthenticationScheme);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> AddOrganization()
        {
            var hasPermission = await _accessManager.HasPermission(User.Identity.Name, _organizationSubsystemName);

            if (hasPermission)
            {
                var users = _userData.GetAllUsers().Select(u => new Select()
                {
                    Id = u.Id,
                    Name = $"{u.LastName} {u.Name} {u.Patronymic}"
                });

                var model = new PostOrganizationViewModel()
                {
                    Users = users
                };

                return View(model);
            }

            return Forbid(CookieAuthenticationDefaults.AuthenticationScheme);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddOrganization(PostOrganizationViewModel model)
        {
            if (ModelState.IsValid)
            {
                var organization = new Organization()
                {
                    Name = model.Name,
                    INN = model.INN,
                    OGRN = model.OGRN
                };

                if (model.SelectedUserId != null)
                {
                    var selectedUserId = (int)model.SelectedUserId;
                    organization.UserId = selectedUserId;

                    var user = await _userData.GetUserData(selectedUserId);
                    organization.User = user;
                }

                _organizationService.Add(organization);

                return RedirectToAction("Index", "Organization");
            }

            return View(model);
        }
    }
}