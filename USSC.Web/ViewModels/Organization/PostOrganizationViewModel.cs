using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace USSC.Web.ViewModels.Organization
{
    public class PostOrganizationViewModel
    {
        [Required(ErrorMessage = "Не указано имя организации")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Не указан ИНН")]
        public string INN { get; set; }

        [Required(ErrorMessage = "Не указан ОГРН")]
        public string OGRN { get; set; }

        public IEnumerable<Select> Users { get; set; }

        public int? SelectedUserId { get; set; }
    }
}
