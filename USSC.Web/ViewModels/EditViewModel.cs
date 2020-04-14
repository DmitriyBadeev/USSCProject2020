using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace USSC.Web.ViewModels
{
    public class EditViewModel
    {
        [Required(ErrorMessage = "Не указан Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Не указано имя")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Не указана фамилия")]
        public string LastName { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Пароль введен неверно")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Выберите роли")]
        public List<RoleOption> Roles { get; set; }
    }
}
