using System.Collections.Generic;
using USSC.Infrastructure.Interfaces;

namespace USSC.Infrastructure.Models
{
    public class Organization : IDataModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string INN { get; set; }

        public string OGRN { get; set; }

        public int? UserId { get; set; }

        public User User { get; set; }

        public List<Employee> Employees { get; set; }
    }
}
