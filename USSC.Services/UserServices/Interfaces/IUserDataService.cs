using System.Collections.Generic;
using System.Threading.Tasks;
using USSC.Infrastructure.Models;

namespace USSC.Services.UserServices.Interfaces
{
    public interface IUserDataService
    {
        IEnumerable<string> GetAllRoles();
        Task<User> GetUserData(string email);
        Task<IEnumerable<string>> GetUserRoles(int userId);
        IEnumerable<User> GetAllUsers();
    }
}