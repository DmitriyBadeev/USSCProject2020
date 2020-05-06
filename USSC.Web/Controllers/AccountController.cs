using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using USSC.Services;
using USSC.Services.PermissionServices;
using USSC.Services.UserServices.Interfaces;
using USSC.Web.ViewModels;
using USSC.Web.ViewModels.Account;
using IAuthorizationService = USSC.Services.UserServices.Interfaces.IAuthorizationService;

namespace USSC.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IRegistrationService _registrationService;
        private readonly IAuthorizationService _authorizationService;
        private readonly IAccessManager _accessManager;
        private readonly IUserDataService _userData;
        private readonly string _adminSubsystem;

        public AccountController(IRegistrationService registrationService, IAuthorizationService authorizationService, 
              IAccessManager accessManager, IUserDataService userData)
        {
            _registrationService = registrationService;
            _authorizationService = authorizationService;
            _accessManager = accessManager;
            _userData = userData;
            _adminSubsystem = Constants.AdminSubsystem;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Registration()
        {
            var hasPermission = await _accessManager.HasPermission(User.Identity.Name, _adminSubsystem);

            if (hasPermission)
            {
                var roles = _userData.GetAllRoles();
                var options = roles.Select(r => new Option() {Name = r}).ToList();
                var viewModel = new RegistrationViewModel() { Roles = options }; 
                return View(viewModel);
            }

            return Forbid(CookieAuthenticationDefaults.AuthenticationScheme);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registration(RegistrationViewModel model)
        {
            if (ModelState.IsValid)
            {
                var roles = model.Roles
                    .FindAll(r => r.IsOptionSelected)
                    .Select(r => r.Name)
                    .ToList();

                if (roles.Count == 0)
                {
                    ModelState.AddModelError("", "Выберите хотя бы одну роль");
                    return View(model);
                }

                var user = await _registrationService.RegisterUser(model.Email, model.Name, model.LastName, model.Password, roles);

                if (user != null)
                {
                    return RedirectToAction("Index", "Admin");
                }
                
                ModelState.AddModelError("", "Пользователь с таким Email уже существует");
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _authorizationService.Authenticate(model.Email, model.Password, HttpContext);

                if (user != null)
                {
                    return RedirectToAction("Index", "Organization");
                }

                ModelState.AddModelError("", "Неверный логин или пароль");
            }

            return View(model);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Edit(int id)
        {
            var hasPermission = await _accessManager.HasPermission(User.Identity.Name, _adminSubsystem);

            if (hasPermission)
            {
                var user = await _userData.GetUserData(id);
                var roles = _userData.GetAllRoles();
                var userRoles = await _userData.GetUserRoles(user.Id);

                var viewModel = new EditViewModel()
                {
                    Email = user.Email,
                    Name = user.Name,
                    LastName = user.LastName,
                    Roles = roles.Select(r => new Option() { Name = r, IsOptionSelected = userRoles.Contains(r) }).ToList()
                };

                return View(viewModel);
            }

            return Forbid(CookieAuthenticationDefaults.AuthenticationScheme);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditViewModel model, int id)
        {
            if (ModelState.IsValid)
            {
                var roles = model.Roles
                    .FindAll(r => r.IsOptionSelected)
                    .Select(r => r.Name)
                    .ToList();

                if (roles.Count == 0)
                {
                    ModelState.AddModelError("", "Выберите хотя бы одну роль");
                    return View(model);
                }

                var user = await _userData.EditUser(id, model.Email, model.Name, model.LastName, model.Password, roles);

                if (user != null)
                {
                    return RedirectToAction("Index", "Admin");
                }

                ModelState.AddModelError("", "Пользователь с таким Email уже существует");
            }

            return View(model);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var hasPermission = await _accessManager.HasPermission(User.Identity.Name, _adminSubsystem);

            if (hasPermission)
            {
                await _userData.DeleteUser(id);

                return RedirectToAction("Index", "Admin");
            }

            return Forbid(CookieAuthenticationDefaults.AuthenticationScheme);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> AddRole()
        {
            var hasPermission = await _accessManager.HasPermission(User.Identity.Name, _adminSubsystem);

            if (hasPermission)
            {
                var subsystems = _accessManager
                    .GetAllSubsystems()
                    .Select(s => new Option() {Name = s})
                    .ToList();

                var viewModel = new PostRoleViewModel()
                {
                    SubsystemAccesses = subsystems
                };
                
                return View(viewModel);
            }

            return Forbid(CookieAuthenticationDefaults.AuthenticationScheme);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddRole(PostRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                var isAdded = await _userData.AddRole(model.Name);
                if (!isAdded)
                {
                    ModelState.AddModelError("", "Роль с таким именем уже существует");
                    return View(model);
                }

                var roleEntity = await _userData.FindRole(model.Name);

                if (roleEntity == null)
                {
                    ModelState.AddModelError("", "Не удалось добавить роль");
                    return View(model);
                }

                var subsystemAccesses = model.SubsystemAccesses
                    .FindAll(o => o.IsOptionSelected)
                    .ToList();

                if (subsystemAccesses.Count == 0)
                {
                    ModelState.AddModelError("", "Выберите хотя бы одну подсистему для доступа");
                    return View(model);
                }

                foreach (var subsystem in subsystemAccesses)
                {
                    _accessManager.IssuePermission(roleEntity, subsystem.Name);
                }

                return RedirectToAction("Index", "Admin");
            }

            return View(model);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> UpdateRole(string name)
        {
            var hasPermission = await _accessManager.HasPermission(User.Identity.Name, _adminSubsystem);

            if (hasPermission)
            {
                var roleEntity = await _userData.FindRole(name);
                var accesses = await _accessManager.GetAccessibleSubsystemsByRole(name);

                var subsystemAccesses = _accessManager
                    .GetAllSubsystems()
                    .Select(s => new Option() { Name = s, IsOptionSelected = accesses.Contains(s)})
                    .ToList();

                ViewData["oldName"] = roleEntity.Name;

                var viewModel = new PostRoleViewModel()
                {
                    Name = roleEntity.Name,
                    SubsystemAccesses = subsystemAccesses
                };
                return View(viewModel);
            }

            return Forbid(CookieAuthenticationDefaults.AuthenticationScheme);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateRole(PostRoleViewModel model, string oldName)
        {
            if (ModelState.IsValid)
            {
                var isUpdate = await _userData.UpdateRoleName(oldName, model.Name);

                if (!isUpdate)
                {
                    ModelState.AddModelError("", "Роль с таким именем уже существует");
                    return View(model);
                }

                var roleEntity = await _userData.FindRole(model.Name);

                await _accessManager.RemoveAllRolePermissions(model.Name);

                var subsystemAccesses = model.SubsystemAccesses
                    .FindAll(o => o.IsOptionSelected)
                    .ToList();

                if (subsystemAccesses.Count == 0)
                {
                    ModelState.AddModelError("", "Выберите хотя бы одну подсистему для доступа");
                    return View(model);
                }

                foreach (var subsystem in subsystemAccesses)
                {
                    _accessManager.IssuePermission(roleEntity, subsystem.Name);
                }

                return RedirectToAction("Index", "Admin");
            }

            return View(model);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> DeleteRole(string name)
        {
            var hasPermission = await _accessManager.HasPermission(User.Identity.Name, _adminSubsystem);

            if (hasPermission)
            {
                await _userData.RemoveRole(name);

                return RedirectToAction("Index", "Admin");
            }

            return Forbid(CookieAuthenticationDefaults.AuthenticationScheme);
        }
    }
}