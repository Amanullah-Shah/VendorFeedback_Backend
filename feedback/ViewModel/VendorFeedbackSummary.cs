using System.ComponentModel.DataAnnotations.Schema;

namespace feedback.ViewModel
{
    public class VendorFeedbackSummary
    {
        public int VendorId { get; set; }
        public string VendorName { get; set; }
        public int QuestionId { get; set; }
        [NotMapped]
        public List<AnswerRecord> AnswerRecord { get; set; }
    }
}
