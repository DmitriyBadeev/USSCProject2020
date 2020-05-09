using System.Collections.Generic;
using System.Threading.Tasks;
using USSC.Infrastructure.Models;

namespace USSC.Services.UserServices.Interfaces
{
    public interface IUserDataService
    {
        IEnumerable<User> GetAllUsersWithRole(Role role);
        Task<bool> UpdateRoleName(string oldName, string newName);
        Task<Role> FindRole(string role);
        Task<bool> AddRole(string name);
        Task RemoveRole(string roleName);
        IEnumerable<string> GetAllRoles();
        Task<User> GetUserData(string email);
        Task<User> GetUserData(int id);
        Task<IEnumerable<string>> GetUserRoles(int userId);
        IEnumerable<User> GetAllUsers();
        Task<User> EditUser(int userId, string email, string phone, string name, string lastName, string patronymic,
            string password, IEnumerable<string> roles);
        Task DeleteUser(int userId);
    }
}