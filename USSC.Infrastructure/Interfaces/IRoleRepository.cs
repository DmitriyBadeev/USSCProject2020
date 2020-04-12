using System.Threading.Tasks;
using USSC.Infrastructure.Models;

namespace USSC.Infrastructure.Interfaces
{
    public interface IRoleRepository : IRepository<Role>
    {
        Task<Role> FindRole(string role);
    }
}