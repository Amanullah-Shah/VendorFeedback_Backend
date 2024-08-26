namespace feedback.ViewModel
{
    public class FeedbackSummary
    {
        public int VendorId { get; set; }
        public string VendorName { get; set; }
        public int TotalCount { get; set; }
        public int PoorCount { get; set; }
        public int NormalCount { get; set; }
        public int GoodCount { get; set; }
        public double PoorPercentage { get; set; }
        public double NormalPercentage { get; set; }
        public double GoodPercentage { get; set; }
    }

}
