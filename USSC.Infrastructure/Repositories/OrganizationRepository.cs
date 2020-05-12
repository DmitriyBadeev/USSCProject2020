using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using USSC.Infrastructure.Interfaces;
using USSC.Infrastructure.Models;

namespace USSC.Infrastructure.Repositories
{
    public class OrganizationRepository : Repository<Organization>, IOrganizationRepository
    {
        private readonly AppDbContext _context;

        public OrganizationRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public List<Employee> GetEmployees(int organizationId)
        {
            return _context.Employees
                .Include(e => e.Organization)
                .Include(e => e.Position)
                .Where(e => e.Organization.Id == organizationId)
                .ToList();
        }

        public User GetOrganizationUser(int organizationId)
        {
            return _context.Organizations
                .Include(o => o.User)
                .SingleOrDefault(o => o.Id == organizationId)
                ?.User;
        }

        public List<Position> GetPositions()
        {
            return _context.Positions.ToList();
        }
    }
}
