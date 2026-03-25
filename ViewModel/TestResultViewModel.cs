namespace ZNOWay.ViewModel
{
    public class TestResultViewModel
    {
        public string TestName { get; set; } = string.Empty;
        public int Score { get; set; }
        public int TotalQuestions { get; set; }
        public int TimeSpent { get; set; }
        public double Percentage => TotalQuestions > 0
            ? Math.Round((double)Score / TotalQuestions * 100, 1)
            : 0;
    }
}