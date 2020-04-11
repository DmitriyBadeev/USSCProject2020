using System.Collections.Generic;
using USSC.Infrastructure.Models;

namespace USSC.Infrastructure.Interfaces
{
    public interface ISubsystemRepository : IRepository<Subsystem>
    {
        void IssuePermission(Role role, Subsystem subsystem);
        Subsystem GetSubsystemByName(string name);
        bool HasPermission(List<Role> roles, Subsystem subsystem);
    }
}