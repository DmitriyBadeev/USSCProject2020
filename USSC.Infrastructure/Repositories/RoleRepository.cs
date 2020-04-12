using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using USSC.Infrastructure.Interfaces;
using USSC.Infrastructure.Models;

namespace USSC.Infrastructure.Repositories
{
    public class RoleRepository : Repository<Role>, IRoleRepository
    {
        private readonly AppDbContext _context;

        public RoleRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Role> FindRole(string role)
        {
            var roleEntity = await _context.Roles.FirstOrDefaultAsync(r => r.Name == role);

            return roleEntity;
        }
    }
}
