using System.Collections.Generic;
using System.Threading.Tasks;
using USSC.Infrastructure.Models;

namespace USSC.Infrastructure.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> FindUserByEmail(string email);
        void AssignRole(Role role, User user);
        Task<Role> FindRole(string role);
        Task<List<Role>> GetUserRoles(int userId);
    }
}