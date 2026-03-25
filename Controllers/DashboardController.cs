using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ZNOWay.Data;

namespace ZNOWay.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly AppDbContext _context;

        public DashboardController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var userIdClaim = User.FindFirst("UserId")?.Value;
            if (!int.TryParse(userIdClaim, out int userId))
                return RedirectToAction("Login", "Account");

            // Статистика користувача
            var results = await _context.UserResults
                .Where(r => r.UserId == userId)
                .Include(r => r.Test)
                .OrderByDescending(r => r.Id)
                .ToListAsync();

            var totalTests = results.Count;
            var avgScore = totalTests > 0
                ? results.Average(r => r.TotalQuestions > 0
                    ? (double)r.Score / r.TotalQuestions * 100 : 0)
                : 0;
            var bestScore = totalTests > 0
                ? results.Max(r => r.TotalQuestions > 0
                    ? (double)r.Score / r.TotalQuestions * 100 : 0)
                : 0;

            // Предмети
            var subjects = await _context.Subjects
                .Include(s => s.Tests)
                .ToListAsync();

            ViewBag.TotalTests = totalTests;
            ViewBag.AvgScore = Math.Round(avgScore, 1);
            ViewBag.BestScore = Math.Round(bestScore, 1);
            ViewBag.Subjects = subjects;
            ViewBag.RecentResults = results.Take(5).ToList();

            return View();
        }
    }
}