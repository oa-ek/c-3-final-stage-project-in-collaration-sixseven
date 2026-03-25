namespace ZNOWay.Models
{
    public class Subject
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public ICollection<Topic> Topics { get; set; } = new List<Topic>();
        public ICollection<Test> Tests { get; set; } = new List<Test>();
    }
}
