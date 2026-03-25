using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ZNOWay.Data;
using ZNOWay.Models;
using Microsoft.AspNetCore.Authorization;

namespace ZNOWay.Controllers
{
    [Authorize]
    public class AnswerOptionsController : Controller
    {
        private readonly AppDbContext _context;

        public AnswerOptionsController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.AnswerOptions.Include(a => a.Question).ToListAsync());
        }

        public IActionResult Create()
        {
            ViewBag.Questions = new SelectList(_context.Questions, "Id", "Text");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(AnswerOption answerOption)
        {
            if (ModelState.IsValid)
            {
                _context.Add(answerOption);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Questions = new SelectList(_context.Questions, "Id", "Text");
            return View(answerOption);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var answerOption = await _context.AnswerOptions.FindAsync(id);
            if (answerOption == null) return NotFound();
            ViewBag.Questions = new SelectList(_context.Questions, "Id", "Text");
            return View(answerOption);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, AnswerOption answerOption)
        {
            if (id != answerOption.Id) return NotFound();
            if (ModelState.IsValid)
            {
                _context.Update(answerOption);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
 





           }
            ViewBag.Questions = new SelectList(_context.Questions, "Id", "Text");
            return View(answerOption);
        }

        public async Task<IActionResult> Delete(int id)
        {
 






           var answerOption = await _context.AnswerOptions.Include(a => a.Question).FirstOrDefaultAsync(a => a.Id == id);
            if (answerOption == null) return NotFound();
            return View(answerOption);
        }

 




       [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var answerOption = await _context.AnswerOptions.FindAsync(id);
 



           if (answerOption != null) _context.AnswerOptions.Remove(answerOption);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }





        public async Task<IActionResult> Details(int id)
        {
            var answerOption = await _context.AnswerOptions.Include(a => a.Question).FirstOrDefaultAsync(a => a.Id == id);
 



           if (answerOption == null) return NotFound();
            return View(answerOption);
        }
  


  }

}

