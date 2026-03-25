using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ZNOWay.Data;
using ZNOWay.Models;

namespace ZNOWay.Controllers
{
    [Authorize]
    public class TestsController : Controller
    {
        private readonly AppDbContext _context;

        public TestsController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var tests = await _context.Tests.Include(t => t.Subject).ToListAsync();
            return View(tests);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewBag.SubjectId = new SelectList(_context.Subjects.ToList(), "Id", "Name");
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Test test)
        {
            ModelState.Remove("Subject");
            ModelState.Remove("Questions");
            ModelState.Remove("UserResults");

            if (ModelState.IsValid)
            {
                _context.Add(test);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.SubjectId = new SelectList(_context.Subjects.ToList(), "Id", "Name", test.SubjectId);
            return View(test);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var test = await _context.Tests.FindAsync(id);
            if (test == null) return NotFound();

            ViewBag.SubjectId = new SelectList(_context.Subjects.ToList(), "Id", "Name", test.SubjectId);
            return View(test);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Test test)
        {
            if (id != test.Id) return NotFound();

            ModelState.Remove("Subject");
            ModelState.Remove("Questions");
            ModelState.Remove("UserResults");

            if (ModelState.IsValid)
            {
                _context.Update(test);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.SubjectId = new SelectList(_context.Subjects.ToList(), "Id", "Name", test.SubjectId);
            return View(test);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var test = await _context.Tests
                .Include(t => t.Subject)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (test == null) return NotFound();

            return View(test);
        }

        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var test = await _context.Tests.FindAsync(id);
            if (test != null) _context.Tests.Remove(test);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Details(int id)
        {
            var test = await _context.Tests
                .Include(t => t.Subject)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (test == null) return NotFound();

            return View(test);
        }
    }
}