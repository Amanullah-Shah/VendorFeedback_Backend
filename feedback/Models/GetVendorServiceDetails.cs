using System.ComponentModel.DataAnnotations;

namespace feedback.Models
{
    public class GetVendorServiceDetails
    {
        [Key]
        public int ServiceId { get; set; }
        public string ServiceName { get; set; }
        public byte[] Icon { get; set; } // Change to appropriate type based on how you handle icons
        public string VendorName { get; set; }
    }

}
