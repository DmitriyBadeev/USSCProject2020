using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace USSC.Web.ViewModels.Employee
{
    public class PostEmployeeViewModel
    {
        [Required(ErrorMessage = "Не указано имя")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Не указана фамилия")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Не указано отчество")]
        public string Patronymic { get; set; }

        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "Не указана эл. почта")]
        public string Email { get; set; }

        [DataType(DataType.PhoneNumber)]
        [Required(ErrorMessage = "Не указан телефон")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Не указана серия паспорта")]
        public string PassportSeries { get; set; }

        [Required(ErrorMessage = "Не указан номер паспорта")]
        public string PassportNumber { get; set; }

        [DataType(DataType.DateTime)]
        [Required(ErrorMessage = "Не указана дата рождения")]
        public DateTime BirthDay { get; set; }

        [Required(ErrorMessage = "Не указан мед. полис")]
        public string MedicalPolicy { get; set; }

        public IEnumerable<Select> Positions { get; set; }

        public int SelectedPositionId { get; set; }

        public int OrganizationId { get; set; }
    }
}
