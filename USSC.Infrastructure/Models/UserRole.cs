using System;
using System.Collections.Generic;
using System.Text;
using USSC.Infrastructure.Interfaces;

namespace USSC.Infrastructure.Models
{
    public class UserRole : IDataModel
    {
        public int Id { get; set; }

        public User User { get; set; }

        public Role Role { get; set; }
    }
}
