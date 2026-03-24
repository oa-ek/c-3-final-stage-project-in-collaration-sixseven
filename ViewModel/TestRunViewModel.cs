namespace ZNOWay.ViewModels
{
    public class TestRunViewModel
    {
        public int TestId { get; set; }
        public string TestName { get; set; } = string.Empty;
        public string SubjectName { get; set; } = string.Empty;
        public int TotalQuestions { get; set; }
        public int? TimeLimit { get; set; }
    }
}