using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using ZNOWay.Data;
using ZNOWay.ViewModels;

namespace ZNOWay.Controllers
{
    [Authorize]
    public class TestRunController : Controller
    {
        private readonly AppDbContext _context;

        public TestRunController(AppDbContext context)
        {
            _context = context;
        }

        // Сторінка перед початком тесту — показує назву, кількість питань, час
        public async Task<IActionResult> Start(int id)
        {
            var test = await _context.Tests
                .Include(t => t.Subject)
                .Include(t => t.Questions)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (test == null) return NotFound();

            var model = new TestRunViewModel
            {
                TestId = test.Id,
                TestName = test.Name,
                SubjectName = test.Subject?.Name ?? "",
                TotalQuestions = test.Questions.Count,
                TimeLimit = test.TimeLimit
            };
            if (test.TimeLimit.HasValue)
                HttpContext.Session.SetInt32($"timelimit_{test.Id}", test.TimeLimit.Value);

            return View(model);
        }

        // GET: /TestRun/Question/5?questionIndex=0
        // Показує одне питання за раз
        public async Task<IActionResult> Question(int testId, int questionIndex = 0)
        {
            var test = await _context.Tests
                .Include(t => t.Questions)
                    .ThenInclude(q => q.AnswerOptions)
                .FirstOrDefaultAsync(t => t.Id == testId);

            if (test == null) return NotFound();

            var questions = test.Questions.ToList();

            if (questionIndex >= questions.Count)
                return RedirectToAction("Finish", new { testId });

            var question = questions[questionIndex];

            var model = new QuestionViewModel
            {
                TestId = testId,
                QuestionId = question.Id,
                QuestionText = question.Text,
                QuestionType = question.Type,
                CurrentNumber = questionIndex + 1,
                TotalQuestions = questions.Count,
                Options = question.AnswerOptions.Select(a => new AnswerOptionViewModel
                {
                    Id = a.Id,
                    Text = a.Text
                }).ToList()
            };

            // Зберігаємо індекс питання в сесії
            HttpContext.Session.SetInt32($"test_{testId}_current", questionIndex);

            return View(model);
        }

        // POST: /TestRun/Answer
        // Приймає відповідь користувача і переходить до наступного питання
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Answer(int testId, int questionId, int questionIndex, List<int> selectedOptions)
        {
            // Зберігаємо відповіді в сесії
            var key = $"test_{testId}_answers_{questionId}";
            var value = string.Join(",", selectedOptions);
            HttpContext.Session.SetString(key, value);

            return RedirectToAction("Question", new { testId, questionIndex = questionIndex + 1 });
        }

        // GET: /TestRun/Finish/5
        // Підраховує результат і зберігає в БД
        public async Task<IActionResult> Finish(int testId)
        {
            var test = await _context.Tests
                .Include(t => t.Questions)
                    .ThenInclude(q => q.AnswerOptions)
                .FirstOrDefaultAsync(t => t.Id == testId);

            if (test == null) return NotFound();

            int score = 0;

            foreach (var question in test.Questions)
            {
                var key = $"test_{testId}_answers_{question.Id}";
                var savedAnswer = HttpContext.Session.GetString(key);

                if (savedAnswer == null) continue;

                var selectedIds = savedAnswer.Split(',')
                    .Where(s => int.TryParse(s, out _))
                    .Select(int.Parse)
                    .ToList();

                var correctIds = question.AnswerOptions
                    .Where(a => a.IsCorrect)
                    .Select(a => a.Id)
                    .ToList();

                if (selectedIds.OrderBy(x => x).SequenceEqual(correctIds.OrderBy(x => x)))
                    score++;
            }

            // Отримуємо UserId з claims
            var userIdClaim = User.FindFirst("UserId")?.Value;
            if (userIdClaim != null && int.TryParse(userIdClaim, out int userId))
            {
                var result = new ZNOWay.Models.UserResult
                {
                    UserId = userId,
                    TestId = testId,
                    Score = score,
                    TimeSpent = 0
                };
                _context.UserResults.Add(result);
                await _context.SaveChangesAsync();
            }

            var model = new ResultViewModel
            {
                TestName = test.Name,
                Score = score,
                TotalQuestions = test.Questions.Count,
                TimeSpent = 0
            };

            return View("Result", model);
        }
    }
}