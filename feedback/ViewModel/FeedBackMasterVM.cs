using feedback.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace feedback.ViewModel
{
    public class FeedBackMasterVM
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int BranchId { get; set; }
        public int RegionId { get; set; }
        public int Review { get; set; }
        public DateTime Date { get; set; }
        public string Comment { get; set; }

        public User? User { get; set; }
        public int VendorMasterId { get; set; }
        [ForeignKey("VendorMasterId")]
        public VendorMaster? VendorMaster { get; set; }
        public Branch? Branch { get; set; }
        public Region? Region { get; set; }
        public ICollection<FeedBackDetailVM>? FeedbackDetails { get; set; }
    }
}
