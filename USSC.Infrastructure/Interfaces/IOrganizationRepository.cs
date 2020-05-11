using System.Collections.Generic;
using USSC.Infrastructure.Models;

namespace USSC.Infrastructure.Interfaces
{
    public interface IOrganizationRepository : IRepository<Organization>
    {
        List<Employee> GetEmployees(int organizationId);
        User GetOrganizationUser(int organizationId);
    }
}