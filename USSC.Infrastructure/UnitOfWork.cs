using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
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
