using System;
using System.Collections.Generic;
using System.Text;
using USSC.Infrastructure.Interfaces;
using USSC.Infrastructure.Models;

namespace USSC.Infrastructure.Repositories
{
    public class OrganizationRepository : Repository<Organization>, IOrganizationRepository
    {
        public OrganizationRepository(AppDbContext context) : base(context)
        {
        }
    }
}
