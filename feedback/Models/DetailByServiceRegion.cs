using System.ComponentModel.DataAnnotations;

namespace feedback.Models
{
    public class DetailByServiceRegion
    {
        [Key]
        public int ServiceId { get; set; }
        public string ServiceName { get; set; }
        public string VendorName { get; set; }
        public int Vendorid { get; set; }
        public DateTime From_Date { get; set; }
        public DateTime To_Date { get; set; }
    }
}
