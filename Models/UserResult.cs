namespace ZNOWay.Models
{
    public class UserResult
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int TestId { get; set; }
        public int Score { get; set; }
        public int TimeSpent { get; set; }
        public User User { get; set; } = null!;
        public Test Test { get; set; } = null!;
    }
}
