using System;

namespace ZNOWay.ViewModels
{
    public class TestResultViewModel
    {
        public int TestId { get; set; }
        public string TestName { get; set; } = string.Empty;
        public int TotalQuestions { get; set; }
        public int CorrectAnswers { get; set; }
        public string TimeSpent { get; set; } = string.Empty;

        // Відсоток правильних відповідей (0–100)
        public double Percentage => TotalQuestions > 0
            ? Math.Round((double)CorrectAnswers / TotalQuestions * 100, 1)
            : 0;

        // Прогноз балу НМТ за шкалою 100–200
        // Формула: 100 + (Percentage / 100) * 100 = 100 + Percentage
        // Тобто 0% → 100 балів, 100% → 200 балів
        public int NMTScore => (int)Math.Round(100 + Percentage);

        // Для зворотної сумісності
        public int Score => (int)Percentage;
    }
}
