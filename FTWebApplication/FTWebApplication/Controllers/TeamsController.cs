#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FTWebApplication;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Authorization;

namespace FTWebApplication.Controllers
{
   [Authorize(Roles ="admin, user")]
    public class TeamsController : Controller
    {
        private readonly Football_Toutnament_LabContext _context;

        public TeamsController(Football_Toutnament_LabContext context)
        {
            _context = context;
        }

        // GET: Teams
        public async Task<IActionResult> Index()
        {
            return View(await _context.Teams.ToListAsync());
        }

        // GET: Teams/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var team = await _context.Teams
                .FirstOrDefaultAsync(m => m.IdTeam == id);
            if (team == null)
            {
                return NotFound();
            }

            //return View(team);
            return RedirectToAction("Index", "Players", new { id = team.IdTeam, name = team.NameTeam});
        }


        // GET: Teams/Matches
        public async Task<IActionResult> Matches(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var team = await _context.Teams
                .FirstOrDefaultAsync(m => m.IdTeam == id);
            if (team == null)
            {
                return NotFound();
            }

            //return View(team);
            return RedirectToAction("Index", "Matches", new { id = team.IdTeam, name = team.NameTeam });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Import(IFormFile fileExcel)
        {
            if (ModelState.IsValid)
            {
                if (fileExcel != null)
                {
                    using (var stream = new FileStream(fileExcel.FileName, FileMode.Create))
                    {
                        await fileExcel.CopyToAsync(stream);
                        using (XLWorkbook workBook = new XLWorkbook(stream, XLEventTracking.Disabled))
                        {                          
                            foreach (IXLWorksheet worksheet in workBook.Worksheets)
                            {
                                Team newteam;
                                var t = (from team in _context.Teams
                                         where team.NameTeam.Contains(worksheet.Name)
                                         select team).ToList();
                                if (t.Count > 0)
                                {
                                    newteam = t[0];
                                }
                                else
                                {
                                    newteam = new Team();
                                    newteam.NameTeam = worksheet.Name;
                                    newteam.Base = worksheet.Name;
                                    newteam.Coach = worksheet.Name;
                                    //newteam.Coach = "from EXCEL";
                                    //додати в контекст
                                    _context.Teams.Add(newteam);
                                }
                                //перегляд усіх рядків                    
                                foreach (IXLRow row in worksheet.RowsUsed().Skip(1))
                                {
                                    try
                                    {
                                        Team team = new Team();
                                        team.NameTeam = row.Cell(1).Value.ToString();
                                        team.Base = row.Cell(6).Value.ToString();
                                        team.Coach = row.Cell(11).Value.ToString();
                                        //team.Team = newteam;
                                        _context.Teams.Add(team);
                                    }
                                    catch (Exception e)
                                    {
                                       

                                    }
                                }
                            }
                        }
                    }
                }

                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }


        public ActionResult Export()
        {
            using (XLWorkbook workbook = new XLWorkbook(XLEventTracking.Disabled))
            {
                var teams = _context.Teams.Include("Teams").ToList();
                foreach (var t in teams)
                {
                    var worksheet = workbook.Worksheets.Add(t.NameTeam);

                    worksheet.Cell("A1").Value = "Назва";
                    worksheet.Cell("B1").Value = "Автор 1";
                    worksheet.Cell("C1").Value = "Автор 2";
                    worksheet.Cell("D1").Value = "Автор 3";
                    worksheet.Cell("E1").Value = "Автор 4";
                    worksheet.Cell("F1").Value = "Категорія";
                    worksheet.Cell("G1").Value = "Інформація";
                    worksheet.Row(1).Style.Font.Bold = true;
                    var players = t.Players.ToList();

                    //нумерація рядків/стовпчиків починається з індекса 1 (не 0)
                    for (int i = 0; i < teams.Count; i++)
                    {
                        worksheet.Cell(i + 2, 1).Value = teams[i].NameTeam;
                        worksheet.Cell(i + 2, 7).Value = teams[i].Base;
                        worksheet.Cell(i + 2, 13).Value = teams[i].Coach;
                        worksheet.Cell(i + 2, 19).Value = teams[i].PositionTeam;
                        worksheet.Cell(i + 2, 25).Value = teams[i].Win;
                        worksheet.Cell(i + 2, 31).Value = teams[i].Defeat;
                        worksheet.Cell(i + 2, 37).Value = teams[i].Draw;
                        worksheet.Cell(i + 2, 43).Value = teams[i].Points;

                        var pl = _context.Players.Where(p => p.IdPlayer == players[i].IdPlayer).Include("Player").ToList();


                        //більше 4-ох нікуди писати
                        int j = 0;
                        foreach (var p in pl)
                        {
                            if (j < 5)
                            {
                                worksheet.Cell(i + 2, j + 2).Value = p.Fio;
                                j++;
                            }
                        }

                    }
                }
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    stream.Flush();

                    return new FileContentResult(stream.ToArray(),
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                    {
                        //змініть назву файла відповідно до тематики Вашого проєкту

                        FileDownloadName = $"library_{DateTime.UtcNow.ToShortDateString()}.xlsx"
                    };
                }
            }
        }


        // GET: Teams/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Teams/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdTeam,NameTeam,Base,Coach,PositionTeam,Win,Defeat,Draw,Points")] Team team)
        {
            if (ModelState.IsValid)
            {
                _context.Add(team);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(team);
        }

        // GET: Teams/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var team = await _context.Teams.FindAsync(id);
            if (team == null)
            {
                return NotFound();
            }
            return View(team);
        }

        // POST: Teams/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdTeam,NameTeam,Base,Coach,PositionTeam,Win,Defeat,Draw,Points")] Team team)
        {
            if (id != team.IdTeam)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(team);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TeamExists(team.IdTeam))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(team);
        }

        // GET: Teams/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var team = await _context.Teams
                .FirstOrDefaultAsync(m => m.IdTeam == id);
            if (team == null)
            {
                return NotFound();
            }

            return View(team);
        }

        // POST: Teams/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var team = await _context.Teams.FindAsync(id);
            _context.Teams.Remove(team);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TeamExists(int id)
        {
            return _context.Teams.Any(e => e.IdTeam == id);
        }
    }
}
