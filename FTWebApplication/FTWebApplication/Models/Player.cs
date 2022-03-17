using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FTWebApplication
{
    public partial class Player
    {
        public int IdPlayer { get; set; }
        
        public int? IdTeamFk { get; set; }
        [Display(Name = "ПІБ")]
        public string? Fio { get; set; }
        [Required(ErrorMessage = "Поле не повинно бути порожнім")]
        [Display(Name = "Вік")]
        public int? Age { get; set; }
        [Display(Name = "Номер")]
        public int? NumberPlayer { get; set; }
        [Display(Name = "Позиція")]
        public string? RolePlayer { get; set; }
        [Display(Name = "Команда")]

        public virtual Team? IdTeamFkNavigation { get; set; }
    }
}
