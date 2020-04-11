using System.Collections.Generic;
using USSC.Infrastructure.Interfaces;

namespace USSC.Infrastructure.Models
{
    public class Role : IDataModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public List<UserRole> Users { get; set; }

        public List<RoleSubsystem> Subsystems { get; set; }
    }
}
