using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace feedback.Models
{
    public class User
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
        public string? ErpId { get; set; }
        public string? Password { get; set; }
        public string? Role { get; set; }
        public int? RegionId { get; set; }
        [ForeignKey("RegionId")]
        public Region? Region { get; set; }
        public DateTime? CreationDate { get; set; }
        public int? BranchId { get; set; }
        [ForeignKey("BranchId")]
        public Branch? Branch { get; set; }

    }
}
