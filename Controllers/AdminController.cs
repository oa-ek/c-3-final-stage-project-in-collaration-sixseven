using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ZNOWay.Data;

namespace ZNOWay.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;

        public AdminController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.UsersCount = await _context.Users.CountAsync();
            ViewBag.SubjectsCount = await _context.Subjects.CountAsync();
            ViewBag.TestsCount = await _context.Tests.CountAsync();
            ViewBag.QuestionsCount = await _context.Questions.CountAsync();
            ViewBag.ResultsCount = await _context.UserResults.CountAsync();
            return View();
        }
    }
}