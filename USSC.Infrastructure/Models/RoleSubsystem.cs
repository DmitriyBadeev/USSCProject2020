using USSC.Infrastructure.Interfaces;

namespace USSC.Infrastructure.Models
{
    public class RoleSubsystem : IDataModel
    {
        public int Id { get; set; } 

        public Role Role { get; set; }

        public Subsystem Subsystem { get; set; }
    }
}
