using ZNOWay.Models;

namespace ZNOWay.ViewModels
{
    public class QuestionViewModel
    {
        public int TestId { get; set; }
        public int QuestionId { get; set; }
        public string QuestionText { get; set; } = string.Empty;
        public QuestionType QuestionType { get; set; }
        public int CurrentNumber { get; set; }
        public int TotalQuestions { get; set; }
        public List<AnswerOptionViewModel> Options { get; set; } = new();
    }

    public class AnswerOptionViewModel
    {
        public int Id { get; set; }
        public string Text { get; set; } = string.Empty;
    }
}