using System.Collections.Generic;

namespace feedback.Models
{
    public class Service
    {
        public int Id { get; set; }
        public string ServiceName { get; set; }
        public DateTime From_Date { get; set; }
        public DateTime To_Date { get; set; }
        public byte[] icon { get; set; }

        public ICollection<VendorDetail> VendorDetails { get; set; }
    }
}
