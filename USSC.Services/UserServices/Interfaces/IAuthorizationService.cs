using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using USSC.Infrastructure.Models;

namespace USSC.Services.UserServices.Interfaces
{
    public interface IAuthorizationService
    {
        Task<User> Authenticate(string email, string password, HttpContext context);
    }
}