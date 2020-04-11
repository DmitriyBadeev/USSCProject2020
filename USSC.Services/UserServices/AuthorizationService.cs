using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using USSC.Infrastructure.Models;
using USSC.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using USSC.Services.UserServices.Interfaces;

namespace USSC.Services.UserServices
{
    public class AuthorizationService : IAuthorizationService
    {
        private readonly IApplicationDataService _dataService;
        private readonly ILogger<AuthorizationService> _logger;

        public AuthorizationService(IApplicationDataService dataService, ILogger<AuthorizationService> logger)
        {
            _dataService = dataService;
            _logger = logger;
        }

        public async Task<User> Authenticate(string email, string password, HttpContext context)
        {
            _logger.LogInformation($"Authenticating user {email}");

            var hashedPassword = Helpers.ComputeHash(password);

            var user = _dataService.Data.Users.GetSingleOrDefault(u => u.Email == email);
            
            if (user == null)
            {
                _logger.LogInformation("User not found");
                return null;
            }

            if (user.Password == hashedPassword)
            {
                var roles = await _dataService.Data.Users.GetUserRoles(user.Id);
                var roleClaims = roles.Select(r => new Claim(ClaimsIdentity.DefaultRoleClaimType, r.Name));

                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, user.Email),
                };

                claims.AddRange(roleClaims);

                var identity = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType,
                    ClaimsIdentity.DefaultRoleClaimType);

                await context.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

                _logger.LogInformation("User authenticated successfully");
                return user;
            }

            _logger.LogInformation($"Password is wrong");
            return null;
        }
    }
}
