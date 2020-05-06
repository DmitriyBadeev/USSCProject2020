using System;
using System.Collections.Generic;
using System.Linq;
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

        public IEnumerable<string> GetAllSubsystems()
        {
            return _applicationData.Data.Subsystems
                .GetAll()
                .Select(s => s.Name);
        }

        public void IssuePermission(Role role, string subsystem)
        {
            _logger.LogInformation($"Issue permission for {role.Name} in {subsystem}");
            var subsystemEntity = _applicationData.Data.Subsystems.GetSubsystemByName(subsystem);

            _applicationData.Data.Subsystems.IssuePermission(role, subsystemEntity);
            _logger.LogInformation($"Permission for {role.Name} in {subsystem} issue successfully");
        }

        public async Task RemoveAllRolePermissions(string roleName)
        {
            _logger.LogInformation($"Removing all permissions for {roleName}");
            var roleEntity = await _applicationData.Data.Roles.FindRole(roleName);

            _applicationData.Data.Subsystems.RemoveAllPermissions(roleEntity);
            await _applicationData.Data.SaveChangesAsync();
            _logger.LogInformation($"Permissions for {roleName} removed successfully");
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

        public async Task<IEnumerable<string>> GetAccessibleSubsystemsByRole(string role)
        {
            var subsystems = _applicationData.Data.Subsystems.GetAll();
            var roleEntity = await _applicationData.Data.Roles.FindRole(role);

            return subsystems
                .Where(s => _applicationData.Data.Subsystems.HasPermission(new List<Role> {roleEntity}, s))
                .Select(s => s.Name);
        }

        public async Task<IEnumerable<string>> GetAccessibleSubsystems(string email)
        {
            var user = await _applicationData.Data.Users.FindUserByEmail(email);
            return await GetAccessibleSubsystems(user.Id);
        }

        public async Task<IEnumerable<string>> GetAccessibleSubsystems(int userId)
        {
            var subsystems = _applicationData.Data.Subsystems.GetAll();
            var roles = await _applicationData.Data.Users.GetUserRoles(userId);

            return subsystems
                .Where(s => _applicationData.Data.Subsystems.HasPermission(roles, s))
                .Select(s => s.Name);
        }

        public async Task<bool> IsAdmin(int userId)
        {
            var roles = await _applicationData.Data.Users.GetUserRoles(userId);
            var adminRole = await _applicationData.Data.Roles.FindRole(Constants.AdminRole);

            return roles.Contains(adminRole);
        }
    }
}
