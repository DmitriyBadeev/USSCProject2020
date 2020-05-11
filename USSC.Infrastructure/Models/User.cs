using System.Collections.Generic;
using USSC.Infrastructure.Interfaces;

namespace USSC.Infrastructure.Models
{
    public class User : IDataModel
    {
        public int Id { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public string Name { get; set; }

        public string LastName { get; set; }

        public string Patronymic { get; set; }

        public string Password { get; set; }

        public List<UserRole> Roles { get; set; }

        public Organization Organization { get; set; }
    }
}
