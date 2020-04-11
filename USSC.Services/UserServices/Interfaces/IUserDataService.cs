using System.Collections.Generic;
using System.Threading.Tasks;
using USSC.Infrastructure.Models;

namespace USSC.Services.UserServices.Interfaces
{
    public interface IUserDataService
    {
        Task<User> GetUserData(string email);
        Task<List<Role>> GetUserRoles(int userId);

        IEnumerable<User> GetAllUsers();
    }
}