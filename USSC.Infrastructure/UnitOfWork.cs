using System.Threading.Tasks;
using USSC.Infrastructure.Interfaces;
using USSC.Infrastructure.Models;
using USSC.Infrastructure.Repositories;

namespace USSC.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        private UserRepository _users;
        private SubsystemRepository _subsystems;
        private RoleRepository _roles;
        private OrganizationRepository _organizations;
        private Repository<Employee> _employees;
        private Repository<Position> _positions;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }

        public IUserRepository Users
        {
            get
            {
                return _users ??= new UserRepository(_context);
            }
        }

        public ISubsystemRepository Subsystems
        {
            get
            {
                return _subsystems ??= new SubsystemRepository(_context);
            }
        }

        public IRoleRepository Roles
        {
            get
            {
                return _roles ??= new RoleRepository(_context);
            }
        }

        public IOrganizationRepository Organizations
        {
            get
            {
                return _organizations ??= new OrganizationRepository(_context);
            }
        }

        public IRepository<Employee> Employees
        {
            get
            {
                return _employees ??= new Repository<Employee>(_context);
            }
        }

        public IRepository<Position> Positions
        {
            get
            {
                return _positions ??= new Repository<Position>(_context);
            }
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
