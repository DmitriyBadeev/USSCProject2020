using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using USSC.Web.ViewModels.Organization;

namespace USSC.Web.ViewModels.Employee
{
    public class EmployeeViewModel
    {
        public string Name { get; set; }

        public string LastName { get; set; }

        public string Patronymic { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public string PassportSeries { get; set; }

        public string PassportNumber { get; set; }

        public DateTime BirthDay { get; set; }

        public string MedicalPolicy { get; set; }

        public string Position { get; set; }

        public OrganizationViewModel Organization { get; set; }
    }
}
