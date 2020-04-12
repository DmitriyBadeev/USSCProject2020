﻿using System.Collections.Generic;
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

        public async Task<User> FindUserByEmail(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
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
    }
}
