using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FTWebApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChartController : ControllerBase
    {
        private readonly Football_Toutnament_LabContext _context;
        public ChartController(Football_Toutnament_LabContext context)
        {
            _context = context;
        }

        [HttpGet("JsonData")]
        public JsonResult JsonData() 
        {
            var teams = _context.Teams.ToList();
            List<object> teamPlayer = new List<object>();
            teamPlayer.Add(new[] { "Команда", "Кількість гравців" });
            foreach (var t in teams) 
            {
                teamPlayer.Add(new object[] { t.NameTeam, t.Players.Count() });
            }
            return new JsonResult(teamPlayer);
        }


        [HttpGet("JsonPointStats")]
        public JsonResult JsonPointStats()
        {
            var teams = _context.Teams.ToList();
            List<object> teamPlayer = new List<object>();
            teamPlayer.Add(new[] { "Команда", "Очки" });
            foreach (var t in teams)
            {
                teamPlayer.Add(new object[] { t.NameTeam, t.Points });
            }
            return new JsonResult(teamPlayer);
        }
    }
}
