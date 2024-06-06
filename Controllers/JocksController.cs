using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using JockWebApp.Data;
using JockWebApp.Models;

namespace JockWebApp.Controllers
{
    [Authorize] // This attribute ensures only authenticated users can access this controller
    public class JocksController : Controller
    {
        private readonly ApplicationDbContext _context;

        public JocksController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Jocks
        public async Task<IActionResult> Index(string searchString)
        {
            ViewData["CurrentFilter"] = searchString;

            var jocks = from j in _context.Jock
                        select j;

            if (!String.IsNullOrEmpty(searchString))
            {
                jocks = jocks.Where(s => s.JockQuestion.Contains(searchString) || s.JockAnswer.Contains(searchString));
            }

            return View(await jocks.ToListAsync());
        }

        // GET: Jocks/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Jocks/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,JockQuestion,JockAnswer")] Jock jock)
        {
            if (ModelState.IsValid)
            {
                _context.Add(jock);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(jock);
        }

        // GET: Jocks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jock = await _context.Jock.FindAsync(id);
            if (jock == null)
            {
                return NotFound();
            }
            return View(jock);
        }

        // POST: Jocks/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,JockQuestion,JockAnswer")] Jock jock)
        {
            if (id != jock.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(jock);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!JockExists(jock.ID))
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
            return View(jock);
        }

        // GET: Jocks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jock = await _context.Jock
                .FirstOrDefaultAsync(m => m.ID == id);
            if (jock == null)
            {
                return NotFound();
            }

            return View(jock);
        }

        // POST: Jocks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var jock = await _context.Jock.FindAsync(id);
            if (jock != null)
            {
                _context.Jock.Remove(jock);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool JockExists(int id)
        {
            return _context.Jock.Any(e => e.ID == id);
        }
    }
}
