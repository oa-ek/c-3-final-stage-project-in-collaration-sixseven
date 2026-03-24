using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ZNOWay.Data;
using ZNOWay.Models;
using Microsoft.AspNetCore.Authorization;

namespace ZNOWay.Controllers
{
    [Authorize]
    public class UserResultsController : Controller
    {
        private readonly AppDbContext _context;

        public UserResultsController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var userIdClaim = User.FindFirst("UserId")?.Value;

            if (User.IsInRole("Admin"))
            {
                var all = await _context.UserResults
                    .Include(r => r.User)
                    .Include(r => r.Test)
                    .ToListAsync();
                return View(all);
            }

            if (int.TryParse(userIdClaim, out int userId))
            {
                var my = await _context.UserResults
                    .Include(r => r.User)
                    .Include(r => r.Test)
                    .Where(r => r.UserId == userId)
                    .ToListAsync();
                return View(my);
            }

            return View(new List<UserResult>());
        }
        public IActionResult Create()
        {
            ViewBag.Users = new SelectList(_context.Users, "Id", "Email");
            ViewBag.Tests = new SelectList(_context.Tests, "Id", "Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(UserResult userResult)
        {
            if (ModelState.IsValid)
            {
                _context.Add(userResult);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Users = new SelectList(_context.Users, "Id", "Email");
            ViewBag.Tests = new SelectList(_context.Tests, "Id", "Name");
            return View(userResult);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var userResult = await _context.UserResults.FindAsync(id);
            if (userResult == null) return NotFound();
            ViewBag.Users = new SelectList(_context.Users, "Id", "Email");
            ViewBag.Tests = new SelectList(_context.Tests, "Id", "Name");
            return View(userResult);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, UserResult userResult)
        {
            if (id != userResult.Id) return NotFound();
 



           if (ModelState.IsValid)
            {
                _context.Update(userResult);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Users = new SelectList(_context.Users, "Id", "Email");
 






           ViewBag.Tests = new SelectList(_context.Tests, "Id", "Name");
            return View(userResult);
        }

        public async Task<IActionResult> Delete(int id)
 




       {
            var userResult = await _context.UserResults.Include(ur => ur.User).Include(ur => ur.Test).FirstOrDefaultAsync(ur => ur.Id == id);
            if (userResult == null) return NotFound();
            return View(userResult);
        }






        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userResult = await _context.UserResults.FindAsync(id);
 




           if (userResult != null) _context.UserResults.Remove(userResult);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }





        public async Task<IActionResult> Details(int id)
        {
 


           var userResult = await _context.UserResults.Include(ur => ur.User).Include(ur => ur.Test).FirstOrDefaultAsync(ur => ur.Id == id);
            if (userResult == null) return NotFound();
            return View(userResult);
 


       }
    }
}