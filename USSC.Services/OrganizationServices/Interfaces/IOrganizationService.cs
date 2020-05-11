using System.Collections.Generic;
using USSC.Infrastructure.Models;

namespace USSC.Services.OrganizationServices
{
    public interface IOrganizationService
    {
        IEnumerable<Organization> GetAll();
        Organization GetById(int id);
        void Add(Organization organization);
        void Update(Organization organization);
        void Delete(int id);
    }
}