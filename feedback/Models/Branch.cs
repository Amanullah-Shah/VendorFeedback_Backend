using System.Collections.Generic;

namespace feedback.Models
{
    public class Branch
    {
        public int Id { get; set; }
        public string BranchCode { get; set; }
        public string BranchName { get; set; }
        public string Address { get; set; }
        public string Status { get; set; }
        public int RegionId { get; set; }

        public Region Region { get; set; }
    }
}
