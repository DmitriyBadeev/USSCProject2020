using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using USSC.Infrastructure.Models;
using USSC.Infrastructure.Services;

namespace USSC.Services.OrganizationServices
{
    public class OrganizationService : IOrganizationService
    {
        private readonly IApplicationDataService _applicationData;
        private readonly ILogger<OrganizationService> _logger;

        public OrganizationService(IApplicationDataService applicationData, ILogger<OrganizationService> logger)
        {
            _applicationData = applicationData;
            _logger = logger;
        }

        public IEnumerable<Organization> GetAll()
        {
            var organizations = _applicationData.Data.Organizations
                .GetAll()
                .Select(o =>
                {
                    if (o.UserId != null)
                    {
                        var userId = (int) o.UserId;
                        var user = _applicationData.Data.Users.FindUserById(userId).Result;
                        o.User = user;
                    }
                    
                    return o;
                });

            return organizations;
        }

        public Organization GetById(int id)
        {
            var organization = _applicationData.Data.Organizations.Get(id);
            organization.Employees = _applicationData.Data.Organizations.GetEmployees(organization.Id);
            organization.User = _applicationData.Data.Organizations.GetOrganizationUser(organization.Id);
            return organization;
        }

        public void Add(Organization organization)
        {
            _logger.LogInformation($"Adding organization {organization.Name}");

            _applicationData.Data.Organizations.Add(organization);
            _applicationData.Data.SaveChanges();

            _logger.LogInformation($"Organization {organization.Name} has added successfully");
        }

        public void Update(Organization organization)
        {
            _logger.LogInformation($"Updating organization {organization.Name}");

            _applicationData.Data.Organizations.Update(organization);
            _applicationData.Data.SaveChanges();

            _logger.LogInformation($"Organization {organization.Name} has updated successfully");
        }

        public void Delete(int id)
        {
            var entity = _applicationData.Data.Organizations.Get(id);
            _logger.LogInformation($"Removing organization {entity.Name}");

            _applicationData.Data.Organizations.Remove(entity);
            _applicationData.Data.SaveChanges();

            _logger.LogInformation($"Organization {entity.Name} has removed successfully");
        }
    }
}