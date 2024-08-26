namespace feedback.Models
{
    public class suggestion_answer
    {
        public int id { get; set; }
        public string name { get; set; }
        public int suggestionId { get; set; }

        // Navigation property
        public suggestion_question suggestion { get; set; }
    }
}
