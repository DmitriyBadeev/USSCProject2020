using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using USSC.Infrastructure.Models;
using USSC.Infrastructure.Services;
using USSC.Services.UserServices.Interfaces;

namespace USSC.Services.UserServices
{
    public class RegistrationService : IRegistrationService
    {
        private readonly IApplicationDataService _dataService;
        private readonly ILogger<RegistrationService> _logger;

        public RegistrationService(IApplicationDataService dataService, ILogger<RegistrationService> logger)
        {
            _dataService = dataService;
            _logger = logger;
        }

        public async Task<User> RegisterUser(string email, string name, string lastName, string password, 
            IEnumerable<string> roles)
        {
            _logger.LogInformation($"Registration user {name} {lastName} - {email}");
            var existUser =_dataService.Data.Users.GetSingleOrDefault(u => u.Email == email);

            if (existUser == null)
            {
                var newUser = new User()
                {
                    Email = email,
                    Name = name,
                    LastName = lastName,
                    Password = Helpers.ComputeHash(password)
                };

                _dataService.Data.Users.Add(newUser);
                await _dataService.Data.SaveChangesAsync();

                _logger.LogInformation($"User {name} {lastName} - {email} registered successfully");

                await AddRoles(newUser, roles);

                return newUser;
            }

            _logger.LogWarning($"User with email {email} already exist");
            return null;
        }

        private async Task AddRoles(User user, IEnumerable<string> roles)
        {
            _logger.LogInformation($"Adding roles for user {user.Name} {user.LastName} - {user.Email}");
            var userEntity = await _dataService.Data.Users.FindUserByEmail(user.Email);

            foreach (var role in roles)
            {
                var roleEntity = await _dataService.Data.Roles.FindRole(role);
                _dataService.Data.Users.AssignRole(roleEntity, userEntity);
                await _dataService.Data.SaveChangesAsync();
            }

            _logger.LogInformation($"Roles Added for user {user.Name} {user.LastName} - {user.Email} successfully");
        }
    }
}
