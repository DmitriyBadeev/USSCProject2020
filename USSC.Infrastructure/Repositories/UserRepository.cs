using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using USSC.Infrastructure.Interfaces;
using USSC.Infrastructure.Models;

namespace USSC.Infrastructure.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public IEnumerable<User> GetAllUsersWithRole(int roleId)
        {
            var users = _context.UserRoles
                .Include(r => r.Role)
                .Include(r => r.User)
                .Where(r => r.Role.Id == roleId)
                .Select(r => r.User);

            return users;
        }

        public async Task<User> FindUserByEmail(string email)
        {
            return await _context.Users
                .Include(u => u.Organization)
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User> FindUserById(int id)
        {
            return await _context.Users
                .Include(u => u.Organization)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public void AssignRole(Role role, User user)
        {
            var connection = new UserRole()
            {
                Role = role,
                User = user
            };

            _context.UserRoles.Add(connection);
        }

        public async Task<List<Role>> GetUserRoles(int userId)
        {
            var roles = await _context.UserRoles
                .Include(u => u.Role)
                .Include(u => u.User)
                .Where(u => u.User.Id == userId)
                .Select(u => u.Role)
                .ToListAsync();

            return roles;
        }

        public void RemoveUserRoles(int userId)
        {
            var userRoles = _context.UserRoles
                .Include(u => u.User)
                .Where(u => u.User.Id == userId);

            _context.UserRoles.RemoveRange(userRoles);
        }

        public void RemoveUserRoles(string roleName)
        {
            var userRoles = _context.UserRoles
                .Include(u => u.Role)
                .Where(u => u.Role.Name == roleName);

            _context.UserRoles.RemoveRange(userRoles);
        }

        public void DeleteUser(int userId)
        {
            RemoveUserRoles(userId);
            _context.Users.Remove(FindUserById(userId).Result);
        }
    }
}
