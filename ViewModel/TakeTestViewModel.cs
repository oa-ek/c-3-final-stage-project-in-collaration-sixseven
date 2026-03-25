using System.Collections.Generic;

namespace ZNOWay.ViewModels
{
    public class TakeTestViewModel
    {
        public int TestId { get; set; }
        public string TestName { get; set; } = string.Empty;

        // "Exam" або "Training"
        public string TestType { get; set; } = "Training";

        // Ліміт часу в хвилинах (тільки для Exam)
        public int TimeLimitMinutes { get; set; } = 30;

        public List<QuestionViewModel> Questions { get; set; } = new();
    }
}
