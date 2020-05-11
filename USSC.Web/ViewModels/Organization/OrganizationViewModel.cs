using System.Collections.Generic;
using USSC.Web.ViewModels.Employee;

namespace USSC.Web.ViewModels.Organization
{
    public class OrganizationViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string INN { get; set; }

        public string OGRN { get; set; }

        public string UserName { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public IEnumerable<EmployeeTableViewModel> Employees { get; set; }
    }
}
