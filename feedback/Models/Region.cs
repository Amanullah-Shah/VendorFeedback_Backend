using System.Collections.Generic;

namespace feedback.Models
{
    public class Region
    {
        public int Id { get; set; }
        public string RegionName { get; set; }

        public ICollection<User> Users { get; set; }
        public ICollection<VendorMaster> VendorMasters { get; set; }
        public ICollection<Branch> Branches { get; set; }
        public ICollection<VendorDetail> VendorDetails { get; set; }
    }
}
