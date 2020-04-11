using System.Collections.Generic;
using System.Threading.Tasks;
using USSC.Infrastructure.Models;

namespace USSC.Services.UserServices.Interfaces
{
    public interface IRegistrationService
    {
        Task<User> RegisterUser(string email, string name, string lastName, string password, IEnumerable<string> roles);
    }
}