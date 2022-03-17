#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FTWebApplication;
using Microsoft.AspNetCore.Authorization;

namespace FTWebApplication.Controllers
{
    [Authorize(Roles = "admin, user")]
    public class staffsController : Controller
    {
        private readonly Football_Toutnament_LabContext _context;

        public staffsController(Football_Toutnament_LabContext context)
        {
            _context = context;
        }

        // GET: staffs
        public async Task<IActionResult> Index()
        {     
            return View(await _context.staff.ToListAsync());
        }

        // GET: staffs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var staff = await _context.staff
                .Include(s => s.IdTeamFkkNavigation)
                .FirstOrDefaultAsync(m => m.IdStaff == id);
            if (staff == null)
            {
                return NotFound();
            }

            return View(staff);
        }

        // GET: staffs/Create
        public IActionResult Create()
        {
            ViewData["IdTeamFkk"] = new SelectList(_context.Teams, "IdTeam", "NameTeam");
            return View();
        }

        // POST: staffs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdStaff,IdTeamFkk,FioStaff,RoleStaff")] staff staff)
        {
            if (ModelState.IsValid)
            {
                _context.Add(staff);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdTeamFkk"] = new SelectList(_context.Teams, "IdTeam", "NameTeam", staff.IdTeamFkk);
            return View(staff);
        }

        // GET: staffs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var staff = await _context.staff.FindAsync(id);
            if (staff == null)
            {
                return NotFound();
            }
            ViewData["IdTeamFkk"] = new SelectList(_context.Teams, "IdTeam", "NameTeam", staff.IdTeamFkk);
            return View(staff);
        }

        // POST: staffs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdStaff,IdTeamFkk,FioStaff,RoleStaff")] staff staff)
        {
            if (id != staff.IdStaff)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(staff);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!staffExists(staff.IdStaff))
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
            ViewData["IdTeamFkk"] = new SelectList(_context.Teams, "IdTeam", "NameTeam", staff.IdTeamFkk);
            return View(staff);
        }

        // GET: staffs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var staff = await _context.staff
                .Include(s => s.IdTeamFkkNavigation)
                .FirstOrDefaultAsync(m => m.IdStaff == id);
            if (staff == null)
            {
                return NotFound();
            }

            return View(staff);
        }

        // POST: staffs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var staff = await _context.staff.FindAsync(id);
            _context.staff.Remove(staff);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool staffExists(int id)
        {
            return _context.staff.Any(e => e.IdStaff == id);
        }
    }
}
