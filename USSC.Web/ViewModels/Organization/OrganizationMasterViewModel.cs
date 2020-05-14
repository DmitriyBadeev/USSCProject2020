using System.Collections.Generic;

namespace USSC.Web.ViewModels.Organization
{
    public class OrganizationMasterViewModel
    {
        public IEnumerable<OrganizationTableViewModel> Organizations { get; set; }

        public string SearchString { get; set; }
    }
}