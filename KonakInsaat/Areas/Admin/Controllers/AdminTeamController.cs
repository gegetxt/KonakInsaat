using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using KonakInsaat.Data;
using KonakInsaat.Areas.Admin.Models;

namespace KonakInsaat.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AllowAnonymous] // İstersen daha sonra [Authorize] yaparız
    public class AdminTeamController : Controller
    {
        private readonly KonakInsaatContext _context;

        public AdminTeamController(KonakInsaatContext context)
        {
            _context = context;
        }

        // GET: Admin/AdminTeam
        public async Task<IActionResult> Index()
        {
            var ekip = await _context.TeamMembers
                .OrderBy(t => t.Sira)
                .ToListAsync();
            return View("AdminTeam", ekip);
        }

        // GET: Admin/AdminTeam/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/AdminTeam/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TeamMember member)
        {
            if (ModelState.IsValid)
            {
                _context.TeamMembers.Add(member);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Ekip üyesi başarıyla eklendi.";
                return RedirectToAction(nameof(Index));
            }

            return View(member);
        }

        // GET: Admin/AdminTeam/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var member = await _context.TeamMembers.FindAsync(id);
            if (member == null) return NotFound();

            return View(member);
        }

        // POST: Admin/AdminTeam/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TeamMember member)
        {
            if (id != member.TeamMemberID) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(member);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Ekip üyesi başarıyla güncellendi.";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.TeamMembers.Any(e => e.TeamMemberID == id))
                        return NotFound();
                    throw;
                }
            }

            return View(member);
        }

        // GET: Admin/AdminTeam/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var member = await _context.TeamMembers.FindAsync(id);
            if (member == null) return NotFound();

            _context.TeamMembers.Remove(member);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Ekip üyesi silindi.";
            return RedirectToAction(nameof(Index));
        }
    }
}














