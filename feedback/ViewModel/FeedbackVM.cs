using feedback.Models;

namespace feedback.ViewModel
{
    public class FeedbackVM
    {
        public int FeedbackId { get; set; }
        public string VendorName { get; set; }
        public int Review { get; set; }
        public DateTime Date { get; set; }
        public string Comment { get; set; }
        public List<QuestionAnswer> Questions { get; set; }
        public List<SuggestionAnswer> Suggestions { get; set; }

        public HashSet<string> QuestionNames { get; set; }
        public HashSet<string> SuggestionQuestionNames { get; set; }

        public FeedbackVM()
        {
            Questions = new List<QuestionAnswer>();
            Suggestions = new List<SuggestionAnswer>();
            QuestionNames = new HashSet<string>();
            SuggestionQuestionNames = new HashSet<string>();
        }
    }
}
