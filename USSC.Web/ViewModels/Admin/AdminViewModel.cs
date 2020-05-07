using System.Collections.Generic;
using USSC.Web.ViewModels.Account;
using USSC.Web.ViewModels.Organization;

namespace USSC.Web.ViewModels.Admin
{
    public class AdminViewModel
    {
        public IEnumerable<UserViewModel> UserViewModels { get; set; }
        public IEnumerable<RoleViewModel> RoleViewModels { get; set; }
        public IEnumerable<OrganizationViewModel> OrganizationViewModels { get; set; }
    }
}
