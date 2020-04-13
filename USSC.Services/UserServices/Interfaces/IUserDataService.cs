using System.Collections.Generic;
using System.Threading.Tasks;
using USSC.Infrastructure.Models;

namespace USSC.Services.UserServices.Interfaces
{
    public interface IUserDataService
    {
        IEnumerable<string> GetAllRoles();
        Task<User> GetUserData(string email);
        Task<User> GetUserData(int id);
        Task<IEnumerable<string>> GetUserRoles(int userId);
        IEnumerable<User> GetAllUsers();
        Task<User> EditUser(int id, string email, string name, string lastName, string password, IEnumerable<string> userRoles, IEnumerable<string> roles);
    }
}