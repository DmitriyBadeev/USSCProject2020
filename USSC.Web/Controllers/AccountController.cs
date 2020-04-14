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
                var options = roles.Select(r => new RoleOption() {RoleName = r}).ToList();
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
                    .Select(r => r.RoleName)
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
                    return RedirectToAction("Index", "Home");
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
                    Roles = roles.Select(r => new RoleOption() { RoleName = r, IsOptionSelected = userRoles.Contains(r) }).ToList()
                };

                return View(viewModel);
            }

            return Forbid(CookieAuthenticationDefaults.AuthenticationScheme);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Edit(EditViewModel model, int id)
        {
            if (ModelState.IsValid)
            {
                var roles = model.Roles
                    .FindAll(r => r.IsOptionSelected)
                    .Select(r => r.RoleName)
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
    }
}