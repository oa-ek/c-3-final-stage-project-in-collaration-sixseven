using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ZNOWay.Data;
using ZNOWay.Models;
using Microsoft.AspNetCore.Authorization;

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
            return View(await _context.Tests.Include(t => t.Subject).ToListAsync());
        }

        public IActionResult Create()
        {
            ViewBag.Subjects = new SelectList(_context.Subjects, "Id", "Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Test test)
        {
            if (ModelState.IsValid)
            {
                _context.Add(test);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Subjects = new SelectList(_context.Subjects, "Id", "Name");
            return View(test);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var test = await _context.Tests.FindAsync(id);
            if (test == null) return NotFound();
            ViewBag.Subjects = new SelectList(_context.Subjects, "Id", "Name");
            return View(test);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Test test)
        {
            if (id != test.Id) return NotFound();
            if (ModelState.IsValid)
            {
                _context.Update(test);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }







            ViewBag.Subjects = new SelectList(_context.Subjects, "Id", "Name");
            return View(test);
        }

        public async Task<IActionResult> Delete(int id)
        {
 





           var test = await _context.Tests.Include(t => t.Subject).FirstOrDefaultAsync(t => t.Id == id);
            if (test == null) return NotFound();
            return View(test);
        }

        [HttpPost, ActionName("Delete")]
 





       public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var test = await _context.Tests.FindAsync(id);
            if (test != null) _context.Tests.Remove(test);
            await _context.SaveChangesAsync();




            return RedirectToAction(nameof(Index));
        }

 



       public async Task<IActionResult> Details(int id)
        {
            var test = await _context.Tests.Include(t => t.Subject).FirstOrDefaultAsync(t => t.Id == id);
            if (test == null) return NotFound();
 



           return View(test);
        }
  

  }

}

