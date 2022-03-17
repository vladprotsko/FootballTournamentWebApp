using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FTWebApplication
{
    public partial class Match
    {
        public int IdMatch { get; set; }
        public int IdTeamHt { get; set; }
        public int? IdTeamAt { get; set; }
        public int? IdStadium { get; set; }
        [Display(Name = "Ціна квитка (Євро)")]
        public decimal? CostTicket { get; set; }
        [Display(Name = "Дата матча")]
        public DateTime? DateMatch { get; set; }
        [Display(Name = "Розігрує м'яч в 1 таймі")]
        public int? BallFirst { get; set; }
        [Display(Name = "Розігрує м'яч в 2 таймі")]
        public int? BallSecond { get; set; }
        [Display(Name = "Стадіон на якому гратимуть")]
        public virtual Stadium? IdStadiumNavigation { get; set; }
        [Display(Name = "Виїздна команда")]
        public virtual Team? IdTeamAtNavigation { get; set; }
        [Display(Name = "Домашня команда")]
        public virtual Team IdTeamHtNavigation { get; set; } = null!;
    }
}
