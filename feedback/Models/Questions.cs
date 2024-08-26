namespace feedback.Models
{
    public class Questions
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int S_id { get; set; }
        public List<Service_answer> Service_answer { get; set; }
    }
}
