#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FTWebApplication;

namespace FTWebApplication.Controllers
{
    public class MatchesController : Controller
    {
        private readonly Football_Toutnament_LabContext _context;

        public MatchesController(Football_Toutnament_LabContext context)
        {
            _context = context;
        }

        // GET: Matches
        public async Task<IActionResult> Index(int? id, string? name)
        {
            if (id == null) return RedirectToAction("Teams", "Index");
            ViewBag.TeamId = id;
            ViewBag.TeamName = name;
            var football_Toutnament_LabContext = _context.Matches.Include(t => t.IdTeamHtNavigation).Include(t => t.IdTeamAtNavigation).Include(t => t.IdStadiumNavigation);

            return View(await football_Toutnament_LabContext.ToListAsync());
        }

        // GET: Matches/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var match = await _context.Matches
                .Include(m => m.IdStadiumNavigation)
                .Include(m => m.IdTeamAtNavigation)
                .Include(m => m.IdTeamHtNavigation)
                .FirstOrDefaultAsync(m => m.IdMatch == id);
            if (match == null)
            {
                return NotFound();
            }

            return View(match);
            
        }

        // GET: Matches/Create
        public IActionResult Create()
        {
            ViewData["IdStadium"] = new SelectList(_context.Stadiums, "IdStadium", "IdStadium");
            ViewData["IdTeamAt"] = new SelectList(_context.Teams, "IdTeam", "NameTeam");
            ViewData["IdTeamHt"] = new SelectList(_context.Teams, "IdTeam", "NameTeam");
            return View();
        }

        // POST: Matches/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdMatch,IdTeamHt,IdTeamAt,IdStadium,CostTicket,DateMatch,BallFirst,BallSecond")] Match match)
        {
            if (ModelState.IsValid)
            {
                _context.Add(match);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdStadium"] = new SelectList(_context.Stadiums, "IdStadium", "IdStadium", match.IdStadium);
            ViewData["IdTeamAt"] = new SelectList(_context.Teams, "IdTeam", "NameTeam", match.IdTeamAt);
            ViewData["IdTeamHt"] = new SelectList(_context.Teams, "IdTeam", "NameTeam", match.IdTeamHt);
            return View(match);
        }

        // GET: Matches/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var match = await _context.Matches.FindAsync(id);
            if (match == null)
            {
                return NotFound();
            }
            ViewData["IdStadium"] = new SelectList(_context.Stadiums, "IdStadium", "IdStadium", match.IdStadium);
            ViewData["IdTeamAt"] = new SelectList(_context.Teams, "IdTeam", "NameTeam", match.IdTeamAt);
            ViewData["IdTeamHt"] = new SelectList(_context.Teams, "IdTeam", "NameTeam", match.IdTeamHt);
            return View(match);
        }

        // POST: Matches/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdMatch,IdTeamHt,IdTeamAt,IdStadium,CostTicket,DateMatch,BallFirst,BallSecond")] Match match)
        {
            if (id != match.IdMatch)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(match);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MatchExists(match.IdMatch))
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
            ViewData["IdStadium"] = new SelectList(_context.Stadiums, "IdStadium", "IdStadium", match.IdStadium);
            ViewData["IdTeamAt"] = new SelectList(_context.Teams, "IdTeam", "NameTeam", match.IdTeamAt);
            ViewData["IdTeamHt"] = new SelectList(_context.Teams, "IdTeam", "NameTeam", match.IdTeamHt);
            return View(match);
        }

        // GET: Matches/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var match = await _context.Matches
                .Include(m => m.IdStadiumNavigation)
                .Include(m => m.IdTeamAtNavigation)
                .Include(m => m.IdTeamHtNavigation)
                .FirstOrDefaultAsync(m => m.IdMatch == id);
            if (match == null)
            {
                return NotFound();
            }

            return View(match);
        }

        // POST: Matches/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var match = await _context.Matches.FindAsync(id);
            _context.Matches.Remove(match);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MatchExists(int id)
        {
            return _context.Matches.Any(e => e.IdMatch == id);
        }
    }
}
