namespace feedback.Models
{
    public class VendorDetail
    {
        public int Id { get; set; }
        public int VendorMasterId { get; set; }
        
        public int ServiceId { get; set; }
        public int RegionId { get; set; }
     

        public VendorMaster VendorMaster { get; set; }
        public Service Service { get; set; }
        public Region Region { get; set; }
      
    }
}
