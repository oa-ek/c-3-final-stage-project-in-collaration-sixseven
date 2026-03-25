namespace ZNOWay.Models
{
    public enum QuestionType { Single, Multiple, Open }
    public class Question
    {
        public int Id { get; set; }
        public string Text { get; set; } = string.Empty;
        public QuestionType Type { get; set; }
        public int TestId { get; set; }
        public Test Test { get; set; } = null!;
        public ICollection<AnswerOption> AnswerOptions { get; set; } = new List<AnswerOption>();
    }
}
