using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using USSC.Infrastructure.Interfaces;
using USSC.Infrastructure.Models;

namespace USSC.Infrastructure.Repositories
{
    public class SubsystemRepository : Repository<Subsystem>, ISubsystemRepository
    {
        private readonly AppDbContext _context;

        public SubsystemRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public void IssuePermission(Role role, Subsystem subsystem)
        {
            var permission = new RoleSubsystem()
            {
                Role = role,
                Subsystem = subsystem
            };

            _context.RoleSubsystems.Add(permission);
            _context.SaveChanges();
        }

        public bool HasPermission(List<Role> roles, Subsystem subsystem)
        {
            foreach (var role in roles)
            {
                var hasPermission = _context.RoleSubsystems
                    .Include(rs => rs.Role)
                    .Include(rs => rs.Subsystem)
                    .Any(rs => rs.Role.Id == role.Id && rs.Subsystem.Id == subsystem.Id);

                if (hasPermission)
                    return true;
            }

            return false;
        }

        public Subsystem GetSubsystemByName(string name)
        {
            return GetSingleOrDefault(s => s.Name == name);
        }
    }
}