using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using USSC.Infrastructure.Models;
using USSC.Infrastructure.Services;
using USSC.Services.UserServices.Interfaces;

namespace USSC.Services.UserServices
{
    public class UserDataService : IUserDataService
    {
        private readonly IApplicationDataService _applicationData;

        public UserDataService(IApplicationDataService applicationData)
        {
            _applicationData = applicationData;
        }

        public async Task<User> GetUserData(string email)
        {
            var user = await _applicationData.Data.Users.FindUserByEmail(email);

            return user;
        }

        public async Task<IEnumerable<string>> GetUserRoles(int userId)
        {
            var roles = await _applicationData.Data.Users.GetUserRoles(userId);

            return roles.Select(r => r.Name);
        }

        public IEnumerable<User> GetAllUsers()
        {
            var users = _applicationData.Data.Users.GetAll();

            return users;
        }
    }
}
