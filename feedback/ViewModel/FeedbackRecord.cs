namespace feedback.ViewModel
{
    public class FeedbackRecord
    {
        public int Id { get; set; }
        public int QuestionId { get; set; }
        public int AnswerId { get; set; }
        public string AnswerName { get; set; }
        public string VendorName { get; set; }
        public int VendorId { get; set; }
    }
}
