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
    public class DailyActivitiesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DailyActivitiesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: DailyActivities
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            return View(await _context.DailyActivity.ToListAsync());
        }

        // GET: DailyActivities/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dailyActivity = await _context.DailyActivity
                .FirstOrDefaultAsync(m => m.DailyActivityId == id);
            if (dailyActivity == null)
            {
                return NotFound();
            }

            return View(dailyActivity);
        }

        // GET: DailyActivities/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: DailyActivities/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DailyActivityId,DailyActivityName")] DailyActivity dailyActivity)
        {
            if (ModelState.IsValid)
            {
                _context.Add(dailyActivity);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(dailyActivity);
        }

        // GET: DailyActivities/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dailyActivity = await _context.DailyActivity.FindAsync(id);
            if (dailyActivity == null)
            {
                return NotFound();
            }
            return View(dailyActivity);
        }

        // POST: DailyActivities/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DailyActivityId,DailyActivityName")] DailyActivity dailyActivity)
        {
            if (id != dailyActivity.DailyActivityId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dailyActivity);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DailyActivityExists(dailyActivity.DailyActivityId))
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
            return View(dailyActivity);
        }

        // GET: DailyActivities/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dailyActivity = await _context.DailyActivity
                .FirstOrDefaultAsync(m => m.DailyActivityId == id);
            if (dailyActivity == null)
            {
                return NotFound();
            }

            return View(dailyActivity);
        }

        // POST: DailyActivities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var dailyActivity = await _context.DailyActivity.FindAsync(id);
            if (dailyActivity != null)
            {
                _context.DailyActivity.Remove(dailyActivity);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DailyActivityExists(int id)
        {
            return _context.DailyActivity.Any(e => e.DailyActivityId == id);
        }
    }
}
