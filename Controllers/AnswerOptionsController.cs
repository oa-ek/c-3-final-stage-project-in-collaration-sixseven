using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ZNOWay.Data;
using ZNOWay.Models;

namespace ZNOWay.Controllers
{
    [Authorize(Roles = "Admin")]
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
            ViewBag.QuestionId = new SelectList(_context.Questions, "Id", "Text");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AnswerOption answerOption)
        {
            ModelState.Remove("Question");

            if (ModelState.IsValid)
            {
                _context.Add(answerOption);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.QuestionId = new SelectList(_context.Questions, "Id", "Text", answerOption.QuestionId);
            return View(answerOption);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var answerOption = await _context.AnswerOptions.FindAsync(id);
            if (answerOption == null) return NotFound();

            ViewBag.QuestionId = new SelectList(_context.Questions, "Id", "Text", answerOption.QuestionId);
            return View(answerOption);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, AnswerOption answerOption)
        {
            if (id != answerOption.Id) return NotFound();

            ModelState.Remove("Question");

            if (ModelState.IsValid)
            {
                _context.Update(answerOption);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.QuestionId = new SelectList(_context.Questions, "Id", "Text", answerOption.QuestionId);
            return View(answerOption);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var answerOption = await _context.AnswerOptions.Include(a => a.Question).FirstOrDefaultAsync(m => m.Id == id);
            if (answerOption == null) return NotFound();

            return View(answerOption);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var answerOption = await _context.AnswerOptions.FindAsync(id);
            if (answerOption != null) _context.AnswerOptions.Remove(answerOption);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}