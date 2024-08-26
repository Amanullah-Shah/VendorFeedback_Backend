using System;
using System.Collections.Generic;

namespace feedback.Models
{
    public class VendorMaster
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Contact { get; set; }
        public string? Address { get; set; }
        public int? RegionId { get; set; }
        public string? Status { get; set; }

        public byte[]? Sla { get; set; }
        public DateTime? CreateDate { get; set; }

        public Region? Region { get; set; }


        public ICollection<VendorDetail> VendorDetails { get; set; }
    }
}
