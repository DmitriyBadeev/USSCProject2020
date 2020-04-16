using System.Collections.Generic;

namespace USSC.Web.ViewModels.Admin
{
    public class AdminViewModel
    {
        public IEnumerable<UserViewModel> UserViewModels { get; set; }
        public IEnumerable<RoleViewModel> RoleViewModels { get; set; }
    }
}
