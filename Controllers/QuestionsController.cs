using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ZNOWay.Data;
using ZNOWay.Models;
using Microsoft.AspNetCore.Authorization;

namespace ZNOWay.Controllers
{
    [Authorize]
    public class QuestionsController : Controller
    {
        private readonly AppDbContext _context;

        public QuestionsController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Questions.Include(q => q.Test).ToListAsync());
        }

        public IActionResult Create()
        {
            ViewBag.Tests = new SelectList(_context.Tests, "Id", "Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Question question)
        {
            if (ModelState.IsValid)
            {
                _context.Add(question);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Tests = new SelectList(_context.Tests, "Id", "Name");
            return View(question);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var question = await _context.Questions.FindAsync(id);
            if (question == null) return NotFound();
            ViewBag.Tests = new SelectList(_context.Tests, "Id", "Name");
            return View(question);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Question question)
        {
            if (id != question.Id) return NotFound();
            if (ModelState.IsValid)
            {
                _context.Update(question);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
 






           ViewBag.Tests = new SelectList(_context.Tests, "Id", "Name");
            return View(question);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var question = await _context.Questions.Include(q => q.Test).FirstOrDefaultAsync(q => q.Id == id);
 






           if (question == null) return NotFound();
            return View(question);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
     





   {
            var question = await _context.Questions.FindAsync(id);
            if (question != null) _context.Questions.Remove(question);
            await _context.SaveChangesAsync();
  



          return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int id)
 



       {
            var question = await _context.Questions.Include(q => q.Test).FirstOrDefaultAsync(q => q.Id == id);
            if (question == null) return NotFound();
 


           return View(question);
        }
  

  }

}

