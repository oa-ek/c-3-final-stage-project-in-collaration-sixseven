using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ZNOWay.Data;
using ZNOWay.ViewModels;

namespace ZNOWay.Controllers
{
    [Authorize]
    public class TestRunController(AppDbContext context) : Controller
    {
        private readonly AppDbContext _context = context;

        // GET: /TestRun/Start/5
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

            return View(model);
        }

        // GET: /TestRun/Question?testId=5&questionIndex=0
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

            HttpContext.Session.SetInt32($"test_{testId}_current", questionIndex);

            return View(model);
        }

        // POST: /TestRun/Answer
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Answer(int testId, int questionId, int questionIndex, List<int> selectedOptions)
        {
            var key = $"test_{testId}_answers_{questionId}";
            var value = string.Join(",", selectedOptions);
            HttpContext.Session.SetString(key, value);

            return RedirectToAction("Question", new { testId, questionIndex = questionIndex + 1 });
        }

        // GET: /TestRun/Finish?testId=5
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

                var selectedIds = savedAnswer
                    .Split(',')
                    .Where(s => int.TryParse(s, out _))
                    .Select(int.Parse)
                    .OrderBy(x => x)
                    .ToList();

                var correctIds = question.AnswerOptions
                    .Where(a => a.IsCorrect)
                    .Select(a => a.Id)
                    .OrderBy(x => x)
                    .ToList();

                if (selectedIds.SequenceEqual(correctIds))
                    score++;
            }

            // Зберігаємо результат в БД
            var userIdClaim = User.FindFirst("UserId")?.Value;
            if (int.TryParse(userIdClaim, out int userId))
            {
                var userResult = new ZNOWay.Models.UserResult
                {
                    UserId = userId,
                    TestId = testId,
                    Score = score,
                    TotalQuestions = test.Questions.Count,
                    TimeSpent = 0
                };
                _context.UserResults.Add(userResult);
                await _context.SaveChangesAsync();
            }

            var resultModel = new ResultViewModel
            {
                TestName = test.Name,
                Score = score,
                TotalQuestions = test.Questions.Count,
                TimeSpent = 0
            };

            return View("Result", resultModel);
        }
    }
}