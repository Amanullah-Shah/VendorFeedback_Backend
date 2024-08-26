using feedback.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace feedback.Models
{
    public class FeedbackMaster
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int BranchId { get; set; }
        public int RegionId { get; set; }
        public int Review { get; set; }
        public DateTime Date { get; set; }
        public string Comment { get; set; }
        public int VendorMasterId { get; set; }
        public ICollection<FeedbackDetail>? FeedbackDetails { get; set; }
    }
}
