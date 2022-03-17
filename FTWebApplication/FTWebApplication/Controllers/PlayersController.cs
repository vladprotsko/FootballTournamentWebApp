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
    public class PlayersController : Controller
    {
        private readonly Football_Toutnament_LabContext _context;

        public PlayersController(Football_Toutnament_LabContext context)
        {
            _context = context;
        }

        // GET: Players
        public async Task<IActionResult> Index(int? id, string? name)
        {
            if (id == null) return RedirectToAction("Teams", "Index");
            ViewBag.TeamId = id;
            ViewBag.TeamName = name;
            var football_Toutnament_LabContext = _context.Players.Where(p => p.IdTeamFk == id).Include(p => p.IdTeamFkNavigation);
            
            return View(await football_Toutnament_LabContext.ToListAsync());
        }

        // GET: Players/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var player = await _context.Players
                .Include(p => p.IdTeamFkNavigation)
                .FirstOrDefaultAsync(m => m.IdPlayer == id);
            if (player == null)
            {
                return NotFound();
            }

            return View(player);
        }

        // GET: Players/Create
        public IActionResult Create()
        {
            ViewData["IdTeamFk"] = new SelectList(_context.Teams, "IdTeam", "NameTeam");
            return View();
        }

        // POST: Players/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdPlayer,IdTeamFk,Fio,Age,NumberPlayer,RolePlayer")] Player player)
        {
            if (ModelState.IsValid)
            {
                _context.Add(player);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdTeamFk"] = new SelectList(_context.Teams, "IdTeam", "NameTeam", player.IdTeamFk);
            return View(player);
        }

        // GET: Players/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var player = await _context.Players.FindAsync(id);
            if (player == null)
            {
                return NotFound();
            }
            ViewData["IdTeamFk"] = new SelectList(_context.Teams, "IdTeam", "NameTeam", player.IdTeamFk);
            return View(player);
        }

        // POST: Players/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdPlayer,IdTeamFk,Fio,Age,NumberPlayer,RolePlayer")] Player player)
        {
            if (id != player.IdPlayer)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(player);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PlayerExists(player.IdPlayer))
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
            ViewData["IdTeamFk"] = new SelectList(_context.Teams, "IdTeam", "NameTeam", player.IdTeamFk);
            return View(player);
        }

        // GET: Players/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var player = await _context.Players
                .Include(p => p.IdTeamFkNavigation)
                .FirstOrDefaultAsync(m => m.IdPlayer == id);
            if (player == null)
            {
                return NotFound();
            }

            return View(player);
        }

        // POST: Players/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var player = await _context.Players.FindAsync(id);
            _context.Players.Remove(player);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PlayerExists(int id)
        {
            return _context.Players.Any(e => e.IdPlayer == id);
        }
    }
}
