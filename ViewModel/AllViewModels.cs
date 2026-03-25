using System;
using System.Collections.Generic;

namespace ZNOWay.ViewModels
{
    // ─── Tests/Index ───────────────────────────────────────
    public class TestsIndexViewModel
    {
        public List<TestCardViewModel> Tests { get; set; } = new();
        public string? SelectedSubject { get; set; }
        public string? SelectedYear { get; set; }
        public string? SelectedMode { get; set; }
        public string? SearchQuery { get; set; }
    }

    public class TestCardViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string SubjectName { get; set; } = string.Empty;
        public string SubjectKey { get; set; } = string.Empty;   // math, ukr, hist ...
        public string SubjectIcon { get; set; } = string.Empty;
        public string TestType { get; set; } = "Training";       // Exam / Training
        public int QuestionCount { get; set; }
        public int TimeLimitMinutes { get; set; }
        public int Year { get; set; }
    }

    // ─── Dashboard ─────────────────────────────────────────
    public class DashboardViewModel
    {
        public int PredictedScore { get; set; } = 142;
        public int TestsCompleted { get; set; }
        public int CorrectAnswers { get; set; }
        public int TotalMinutes { get; set; }
        public int MistakesCount { get; set; }
        public int FavoritesCount { get; set; }
        public List<SubjectProgressViewModel> SubjectProgress { get; set; } = new();
        public List<RecentTestViewModel> RecentTests { get; set; } = new();
    }

    public class SubjectProgressViewModel
    {
        public string Icon { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public int Percent { get; set; }
    }

    public class RecentTestViewModel
    {
        public int TestId { get; set; }
        public string TestName { get; set; } = string.Empty;
        public string Mode { get; set; } = string.Empty;
        public int ScorePercent { get; set; }
        public string TimeSpent { get; set; } = string.Empty;
        public string Date { get; set; } = string.Empty;
    }

    // ─── Mistakes ──────────────────────────────────────────
    public class MistakesViewModel
    {
        public List<MistakeItemViewModel> Mistakes { get; set; } = new();
        public string? SelectedSubject { get; set; }
    }

    public class MistakeItemViewModel
    {
        public int QuestionId { get; set; }
        public string SubjectIcon { get; set; } = string.Empty;
        public string SubjectName { get; set; } = string.Empty;
        public string SubjectBadgeClass { get; set; } = "badge-blue";
        public string QuestionText { get; set; } = string.Empty;
        public string UserAnswer { get; set; } = string.Empty;
        public string CorrectAnswer { get; set; } = string.Empty;
        public string Explanation { get; set; } = string.Empty;
    }

    // ─── Favorites ─────────────────────────────────────────
    public class FavoritesViewModel
    {
        public List<FavoriteItemViewModel> Favorites { get; set; } = new();
    }

    public class FavoriteItemViewModel
    {
        public int QuestionId { get; set; }
        public string SubjectIcon { get; set; } = string.Empty;
        public string SubjectName { get; set; } = string.Empty;
        public string SubjectBadgeClass { get; set; } = "badge-blue";
        public string QuestionText { get; set; } = string.Empty;
        public string Meta { get; set; } = string.Empty;
    }

    // ─── Admin ─────────────────────────────────────────────
    public class AdminDashboardViewModel
    {
        public int TotalUsers { get; set; }
        public int TotalTests { get; set; }
        public int TotalQuestions { get; set; }
        public int TotalResults { get; set; }
    }

}
