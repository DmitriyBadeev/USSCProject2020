using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using USSC.Infrastructure.Models;

namespace USSC.Infrastructure
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public DbSet<Role> Roles { get; set; }

        public DbSet<UserRole> UserRoles { get; set; }

        public DbSet<Subsystem> Subsystems { get; set; }

        public DbSet<RoleSubsystem> RoleSubsystems { get; set; }
        
        public DbSet<Employee> Employees { get; set; }

        public DbSet<Position> Positions { get; set; }

        public DbSet<Organization> Organizations { get; set; }

        public AppDbContext(DbContextOptions options) : base(options) { }
    }
}
