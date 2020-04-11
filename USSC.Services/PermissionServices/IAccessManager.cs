using System.Collections.Generic;
using System.Threading.Tasks;

namespace USSC.Services.PermissionServices
{
    public interface IAccessManager
    {
        Task<bool> HasPermission(string email, string subsystemName);
        Task<bool> HasPermission(int userId, string subsystemName);
        Task<IEnumerable<string>> GetAccessibleSubsystems(string email);
        Task<IEnumerable<string>> GetAccessibleSubsystems(int userId);
    }
}