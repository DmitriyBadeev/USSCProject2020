using System;
using System.Collections.Generic;
using USSC.Infrastructure.Interfaces;

namespace USSC.Infrastructure.Models
{
    public class Employee : IDataModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string LastName { get; set; }

        public string Patronymic { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public string PassportSeries { get; set; }

        public string PassportNumber { get; set; }

        public DateTime BirthDay { get; set; }

        public string MedicalPolicy { get; set; }

        public List<Position> Positions { get; set; }

        public Organization Organization { get; set; }
    }
}
