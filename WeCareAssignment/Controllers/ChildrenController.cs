using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WeCareAssignment.Data;
using WeCareAssignment.Models;

namespace WeCareAssignment.Controllers
{
    [Authorize]  
    public class ChildrenController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ChildrenController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Children
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Child
                .Include(c => c.Parents)
                .Include(c => c.Teachers)
                .Include(c => c.DailyActivities);

            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Children/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var child = await _context.Child
                .Include(c => c.Parents)
                .Include(c => c.Teachers)
                .Include(c => c.DailyActivities)
                .FirstOrDefaultAsync(m => m.childId == id);
            if (child == null)
            {
                return NotFound();
            }

            return View(child);
        }

        // GET: Children/Create
        public IActionResult Create()
        {
            ViewData["ParentId"] = new SelectList(_context.Parent.OrderBy(c => c.ParentName), "ParentId", "ParentName");
            ViewData["TeacherId"] = new SelectList(_context.Teacher.OrderBy(c => c.TeacherName), "TeacherId", "TeacherName");
            ViewData["DailyActivityId"] = new SelectList(_context.DailyActivity.OrderBy(c => c.DailyActivityName), "DailyActivityId", "DailyActivityName");
            return View();
        }

        // POST: Children/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("childId,childName,ParentId,TeacherId,DailyActivityId")] Child child)
        {
            if (ModelState.IsValid)
            {
                _context.Add(child);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["ParentId"] = new SelectList(_context.Parent, "ParentId", "ParentId", child.ParentId);
            ViewData["TeacherId"] = new SelectList(_context.Parent, "TeacherId", "TeacherId", child.TeacherId);
            ViewData["DailyActivityId"] = new SelectList(_context.Parent, "DailyActivityId", "DailyActivityId", child.DailyActivityId);
            return View(child);
        }

        // GET: Children/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var child = await _context.Child.FindAsync(id);
            if (child == null)
            {
                return NotFound();
            }
            return View(child);
        }

        // POST: Children/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("childId,childName,ParentId,TeacherId,DailyActivityId")] Child child)
        {
            if (id != child.childId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(child);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ChildExists(child.childId))
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
            return View(child);
        }

        // GET: Children/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var child = await _context.Child
                .FirstOrDefaultAsync(m => m.childId == id);
            if (child == null)
            {
                return NotFound();
            }

            return View(child);
        }

        // POST: Children/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var child = await _context.Child.FindAsync(id);
            if (child != null)
            {
                _context.Child.Remove(child);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ChildExists(int id)
        {
            return _context.Child.Any(e => e.childId == id);
        }
    }
}
