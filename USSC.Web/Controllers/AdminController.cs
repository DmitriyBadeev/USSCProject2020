using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using USSC.Services;
using USSC.Services.LogServices;
using USSC.Services.OrganizationServices;
using USSC.Services.PermissionServices;
using USSC.Services.UserServices.Interfaces;
using USSC.Web.ViewModels.Admin;
using USSC.Web.ViewModels.Position;

namespace USSC.Web.Controllers
{
    public class AdminController : Controller
    {
        private readonly IAccessManager _accessManager;
        private readonly IUserDataService _userDataService;
        private readonly IEventLogService _eventLogService;
        private readonly IPositionService _positionService;
        private readonly IHostEnvironment _hostEnvironment;
        private readonly string _adminSubsystemName;

        public AdminController(IAccessManager accessManager, IUserDataService userDataService, 
            IEventLogService eventLogService, IPositionService positionService, IHostEnvironment hostEnvironment)
        {
            _accessManager = accessManager;
            _userDataService = userDataService;
            _eventLogService = eventLogService;
            _positionService = positionService;
            _hostEnvironment = hostEnvironment;
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
                var roles = _userDataService.GetAllRoles();
                var positions = _positionService.GetAll();

                var userViewModel = users.Select(u => new UserViewModel()
                {
                    Id = u.Id,
                    Email = u.Email,
                    Name = $"{u.LastName} {u.Name} {u.Patronymic}",
                    Roles = string.Join(", ", _userDataService.GetUserRoles(u.Id).Result),
                    Accesses = string.Join(", ", _accessManager.GetAccessibleSubsystems(u.Id).Result)
                });

                var roleViewModel = roles.Select(r => new RoleViewModel()
                {
                    Name = r,
                    AccessibleSubsystems = string.Join(", ", _accessManager.GetAccessibleSubsystemsByRole(r).Result)
                });

                var positionViewModel = positions.Select(p => new PositionViewModel()
                {
                    Id = p.Id,
                    Name = p.Name
                });

                var adminViewModel = new AdminViewModel()
                {
                    RoleViewModels = roleViewModel,
                    UserViewModels = userViewModel,
                    PositionViewModels = positionViewModel
                };
                
                return View(adminViewModel);
            }

            return Forbid(CookieAuthenticationDefaults.AuthenticationScheme);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> EventLog(int current = 0)
        {
            var hasPermission = await _accessManager.HasPermission(User.Identity.Name, _adminSubsystemName);

            if (hasPermission)
            {
                var pathToLogs = Path.Combine(_hostEnvironment.ContentRootPath, "Logs");

                var dates = _eventLogService.GetAllLogDates(pathToLogs);
                dates.Sort((time1, time2) => -time1.CompareTo(time2));

                var logs = _eventLogService.GetLogs(pathToLogs, dates[current]);

                var model = new EventLogViewModel()
                {
                    CurrentDate = current,
                    Dates = dates,
                    Logs = logs
                };

                return View(model);
            }

            return Forbid(CookieAuthenticationDefaults.AuthenticationScheme);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> DeletePosition(int id)
        {
            var hasPermission = await _accessManager.HasPermission(User.Identity.Name, _adminSubsystemName);

            if (hasPermission)
            {
                _positionService.Delete(id);

                return RedirectToAction("Index", "Admin");
            }

            return Forbid(CookieAuthenticationDefaults.AuthenticationScheme);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> EditPosition(int id)
        {
            var hasPermission = await _accessManager.HasPermission(User.Identity.Name, _adminSubsystemName);

            if (hasPermission)
            {
                var position = _positionService.GetById(id);

                var positionViewModel = new PositionViewModel()
                {
                    Id = position.Id,
                    Name = position.Name
                };

                return View(positionViewModel);
            }

            return Forbid(CookieAuthenticationDefaults.AuthenticationScheme);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPosition(int id, PositionViewModel positionModel)
        {
            if (ModelState.IsValid)
            {
                _positionService.Edit(id, positionModel.Name);

                return RedirectToAction("Index", "Admin");
            }

            return View(positionModel);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> AddPosition()
        {
            var hasPermission = await _accessManager.HasPermission(User.Identity.Name, _adminSubsystemName);

            if (hasPermission)
            {
                var positionViewModel = new PositionViewModel();

                return View(positionViewModel);
            }

            return Forbid(CookieAuthenticationDefaults.AuthenticationScheme);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPosition(PositionViewModel positionModel)
        {
            if (ModelState.IsValid)
            {
                _positionService.Add(positionModel.Name);

                return RedirectToAction("Index", "Admin");
            }

            return View(positionModel);
        }
    }
}