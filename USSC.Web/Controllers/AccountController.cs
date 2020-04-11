using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using USSC.Services.UserServices.Interfaces;
using USSC.Web.ViewModels;

namespace USSC.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IRegistrationService _registrationService;
        private readonly IAuthorizationService _authorizationService;

        public AccountController(IRegistrationService registrationService, IAuthorizationService authorizationService)
        {
            _registrationService = registrationService;
            _authorizationService = authorizationService;
        }

        [HttpGet]
        public IActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registration(RegistrationViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _registrationService.RegisterUser(model.Email, model.Name, model.LastName, model.Password, 
                    new List<string> { "Пользователь" });

                if (user != null)
                {
                    await _authorizationService.Authenticate(model.Email, model.Password, HttpContext);
                    return RedirectToAction("Index", "Home");
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
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }
    }
}