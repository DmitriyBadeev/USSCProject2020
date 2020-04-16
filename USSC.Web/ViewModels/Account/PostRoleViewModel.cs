using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace USSC.Web.ViewModels.Account
{
    public class PostRoleViewModel
    {
        [Required(ErrorMessage = "Не указано имя")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Укажите доступы к подсистемам")]
        public List<Option> SubsystemAccesses { get; set; }
    }
}
