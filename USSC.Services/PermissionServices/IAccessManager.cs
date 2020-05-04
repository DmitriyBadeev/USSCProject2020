using System.Collections.Generic;
using System.Threading.Tasks;
using USSC.Infrastructure.Models;

namespace USSC.Services.PermissionServices
{
    public interface IAccessManager
    {
        IEnumerable<string> GetAllSubsystems();
        void IssuePermission(Role role, string subsystem);
        Task<bool> HasPermission(string email, string subsystemName);
        Task<bool> HasPermission(int userId, string subsystemName);
        Task RemoveAllRolePermissions(string roleName);
        Task<IEnumerable<string>> GetAccessibleSubsystems(string email);
        Task<IEnumerable<string>> GetAccessibleSubsystems(int userId);
        Task<IEnumerable<string>> GetAccessibleSubsystemsByRole(string role);
        Task<bool> IsAdmin(int userId);
    }
}