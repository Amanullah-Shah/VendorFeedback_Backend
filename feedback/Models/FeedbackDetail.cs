using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace feedback.Models
{
    public class FeedbackDetail
    {

        [Key]
        public int Id { get; set; }
        public int FeedbackMasterId { get; set; }
       // public FeedbackMaster? FeedbackMaster { get; set; }
        public int QuestionId { get; set; }
        [ForeignKey("QuestionId")]
        public Questions? Question { get; set; }
        public int AnswerID { get; set; }
        [ForeignKey("AnswerID")]
        public Service_answer? ServiceAnswer { get; set; }
        public int suggestionID { get; set; }
        [ForeignKey("suggestionID")]
        public suggestion_question? SuggestionQuestion { get; set; }
        public int suggestion_answerID { get; set; }
        [ForeignKey("suggestion_answerID")]
        public suggestion_answer? SuggestionAnswer { get; set; }





    }
}