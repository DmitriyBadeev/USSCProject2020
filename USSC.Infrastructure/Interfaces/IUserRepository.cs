using System.Collections.Generic;
using System.Threading.Tasks;
using USSC.Infrastructure.Models;

namespace USSC.Infrastructure.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> FindUserByEmail(string email);
        Task<User> FindUserById(int id);
        void AssignRole(Role role, User user);
        Task<List<Role>> GetUserRoles(int userId);
        void RemoveUserRoles(int userId);
        void DeleteUser(int userId);
    }
}