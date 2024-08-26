using feedback.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace feedback.ViewModel
{
    public class FeedBackDetailVM
    {
        [Key]
        public int Id { get; set; }

        public int FeedbackMasterId { get; set; }
        [ForeignKey("FeedbackMasterId")]
        public FeedbackMaster FeedbackMaster { get; set; }

        public int QuestionId { get; set; }
        [ForeignKey("QuestionId")]
        public Questions Question { get; set; }

        public int AnswerID { get; set; }
        [ForeignKey("AnswerID")]
        public Service_answer Answer { get; set; }

        public int suggestionID { get; set; }
        [ForeignKey("suggestionID")]
        public suggestion_question suggestion { get; set; }

        public int suggestion_answerID { get; set; }
        [ForeignKey("suggestion_answerID")]
        public suggestion_answer suggestion_answer { get; set; }
    }
}
