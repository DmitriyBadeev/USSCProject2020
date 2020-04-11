using System.Collections.Generic;
using USSC.Infrastructure.Interfaces;

namespace USSC.Infrastructure.Models
{
    public class Subsystem : IDataModel
    {
        public int Id { get; set; } 

        public string Name { get; set; }

        public List<RoleSubsystem> Roles { get; set; }
    }
}
