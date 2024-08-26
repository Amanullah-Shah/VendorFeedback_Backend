using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace feedback.Models
{
    public class FeedbackDetail
    {

        [Key]
        public int Id { get; set; }
        public int FeedbackMasterId { get; set; }
        public int QuestionId { get; set; }
        public int AnswerID { get; set; }
        public int suggestionID { get; set; }
        public int suggestion_answerID { get; set; }
       





    }
}