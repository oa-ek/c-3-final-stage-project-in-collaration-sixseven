namespace ZNOWay.Models
{
    public enum TestType { Exam, Training }
    public class Test
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int SubjectId { get; set; }
        public int? TimeLimit { get; set; }
        public TestType Type { get; set; }
        public Subject Subject { get; set; } = null!;
        public ICollection<Question> Questions { get; set; } = new List<Question>();
        public ICollection<UserResult> UserResults { get; set; } = new List<UserResult>();
    }
}
