using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using USSC.Infrastructure.Models;
using USSC.Infrastructure.Services;

namespace USSC.Services.PermissionServices
{
    public class AccessManager : IAccessManager
    {
        private readonly IApplicationDataService _applicationData;
        private readonly ILogger<AccessManager> _logger;

        public AccessManager(IApplicationDataService applicationData, ILogger<AccessManager> logger)
        {
            _applicationData = applicationData;
            _logger = logger;
        }

        public async Task<bool> HasPermission(string email, string subsystemName)
        {
            var user = await _applicationData.Data.Users.FindUserByEmail(email);
            return await HasPermission(user.Id, subsystemName);
        }

        public async Task<bool> HasPermission(int userId, string subsystemName)
        {
            _logger.LogInformation($"Checking permission for user with id - {userId} in subsystem - {subsystemName}");

            var subsystem = _applicationData.Data.Subsystems.GetSubsystemByName(subsystemName);

            if (subsystem == null)
            {
                throw new NullReferenceException("Subsystem has not found");
            }

            var roles = await _applicationData.Data.Users.GetUserRoles(userId);
            var hasPermission = _applicationData.Data.Subsystems.HasPermission(roles, subsystem);

            if (hasPermission)
            {
                _logger.LogInformation($"User with id - {userId} has permission for access in subsystem {subsystemName}");
                return true;
            }

            _logger.LogInformation($"User with id - {userId} has not permission for access in subsystem {subsystemName}");
            return false;
        }

        public async Task<List<string>> GetAccessibleSubsystems(string email)
        {
            var user = await _applicationData.Data.Users.FindUserByEmail(email);
            return await GetAccessibleSubsystems(user.Id);
        }

        public async Task<List<string>> GetAccessibleSubsystems(int userId)
        {
            var subsystems = _applicationData.Data.Subsystems.GetAll();
            var roles = await _applicationData.Data.Users.GetUserRoles(userId);

            var accessibleSubsystems = new List<string>();

            foreach (var subsystem in subsystems)
            {
                var hasPermission = _applicationData.Data.Subsystems.HasPermission(roles, subsystem);

                if (hasPermission)
                {
                    accessibleSubsystems.Add(subsystem.Name);
                }
            }

            return accessibleSubsystems;
        }
    }
}
