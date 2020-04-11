using System.Collections.Generic;
using System.Threading.Tasks;

namespace USSC.Services.PermissionServices
{
    public interface IAccessManager
    {
        Task<bool> HasPermission(string email, string subsystemName);
        Task<bool> HasPermission(int userId, string subsystemName);
        Task<List<string>> GetAccessibleSubsystems(string email);
        Task<List<string>> GetAccessibleSubsystems(int userId);
    }
}