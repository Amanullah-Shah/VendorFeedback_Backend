namespace feedback.Models
{
    public class Service_answer
    {
        public int id { get; set; }
        public string name { get; set; }
        public int QuestionsId { get; set; }

        // Navigation property
        public Questions Question { get; set; }
        public ICollection<FeedbackDetail> FeedbackDetails { get; set; }

    }
}
