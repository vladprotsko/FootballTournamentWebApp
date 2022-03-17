using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FTWebApplication
{
    public partial class staff
    {
        public int IdStaff { get; set; }
        public int? IdTeamFkk { get; set; }
        [Display(Name = "ПІБ")]
        public string? FioStaff { get; set; }
        [Required(ErrorMessage = "Поле не повинно бути порожнім")]
        [Display(Name = "Роль")]
        public string? RoleStaff { get; set; }
        [Display(Name = "Команда")]

        public virtual Team? IdTeamFkkNavigation { get; set; }
    }
}
