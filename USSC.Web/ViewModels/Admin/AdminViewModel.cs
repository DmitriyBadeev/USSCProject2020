using System.Collections.Generic;
using USSC.Web.ViewModels.Account;
using USSC.Web.ViewModels.Organization;
using USSC.Web.ViewModels.Position;

namespace USSC.Web.ViewModels.Admin
{
    public class AdminViewModel
    {
        public IEnumerable<UserViewModel> UserViewModels { get; set; }
        public IEnumerable<RoleViewModel> RoleViewModels { get; set; }
        public IEnumerable<PositionViewModel> PositionViewModels { get; set; }

        public string UserSearch { get; set; } 
        public string RoleSearch { get; set; }
        public string PositionSearch { get; set; }
    }
}
