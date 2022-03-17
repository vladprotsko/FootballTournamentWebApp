using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FTWebApplication
{
    public partial class Team
    {
        public Team()
        {
            MatchIdTeamAtNavigations = new HashSet<Match>();
            MatchIdTeamHtNavigations = new HashSet<Match>();
            Players = new HashSet<Player>();
            staff = new HashSet<staff>();
        }

        public int IdTeam { get; set; }
        [Required(ErrorMessage = "Поле не повинно бути порожнім")]
        [Display(Name = "Назва")]

        public string? NameTeam { get; set; }
        [Display(Name = "База")]

        public string? Base { get; set; }
        [Display(Name = "Тренер")]

        public string? Coach { get; set; }
        [Display(Name = "Позиція в таблиці")]

        public int? PositionTeam { get; set; }
            [Display(Name = "Перемоги")]

        public int? Win { get; set; }
        [Display(Name = "Поразки")]

        public int? Defeat { get; set; }
        [Display(Name = "Нічиї")]

        public int? Draw { get; set; }
        [Display(Name = "Очки")]

        public int? Points { get; set; }

        public virtual ICollection<Match> MatchIdTeamAtNavigations { get; set; }
        public virtual ICollection<Match> MatchIdTeamHtNavigations { get; set; }
        public virtual ICollection<Player> Players { get; set; }
        public virtual ICollection<staff> staff { get; set; }
    }
}
