using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ZNOWay.Data;
using ZNOWay.Models;
using Microsoft.AspNetCore.Authorization;

namespace ZNOWay.Controllers
{
    [Authorize]
    public class TopicsController : Controller
    {
        private readonly AppDbContext _context;

        public TopicsController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Topics.Include(t => t.Subject).ToListAsync());
        }

        public IActionResult Create()
        {
            ViewBag.Subjects = new SelectList(_context.Subjects, "Id", "Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Topic topic)
        {
            if (ModelState.IsValid)
            {
                _context.Add(topic);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Subjects = new SelectList(_context.Subjects, "Id", "Name");
            return View(topic);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var topic = await _context.Topics.FindAsync(id);
            if (topic == null) return NotFound();
            ViewBag.Subjects = new SelectList(_context.Subjects, "Id", "Name");
            return View(topic);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Topic topic)
        {
            if (id != topic.Id) return NotFound();
 
           if (ModelState.IsValid)
            {
                _context.Update(topic);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Subjects = new SelectList(_context.Subjects, "Id", "Name");
            return View(topic);
 







       }

        public async Task<IActionResult> Delete(int id)
        {
            var topic = await _context.Topics.Include(t => t.Subject).FirstOrDefaultAsync(t => t.Id == id);
            if (topic == null) return NotFound();
 





           return View(topic);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
 





           var topic = await _context.Topics.FindAsync(id);
            if (topic != null) _context.Topics.Remove(topic);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }






        public async Task<IActionResult> Details(int id)
        {
            var topic = await _context.Topics.Include(t => t.Subject).FirstOrDefaultAsync(t => t.Id == id);
 



           if (topic == null) return NotFound();
            return View(topic);
        }
  


  }

}

