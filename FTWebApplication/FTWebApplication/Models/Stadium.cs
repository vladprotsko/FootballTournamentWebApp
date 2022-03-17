using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FTWebApplication
{
    public partial class Stadium
    {
        public Stadium()
        {
            Matches = new HashSet<Match>();
        }

        public int IdStadium { get; set; }
        [Display(Name = "Назва")]
        public string? Title { get; set; }
        [Required(ErrorMessage = "Поле не повинно бути порожнім")]
        [Display(Name = "Місто")]
        public string? City { get; set; }
        [Display(Name = "Вмісткість")]
        public int? Capacity { get; set; }

        public virtual ICollection<Match> Matches { get; set; }
    }
}
