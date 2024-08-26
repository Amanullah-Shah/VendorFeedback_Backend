namespace feedback.Models
{
    public class suggestion_question
    {
        public int id { get; set; }
        public string name { get; set; }
        public int s_id { get; set; }
        public List<suggestion_answer> suggestion_answer { get; set; }
    }
}
