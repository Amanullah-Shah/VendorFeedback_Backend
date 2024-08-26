using feedback.Models;
using feedback.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace feedback.Data
{
    public class AppDbContext : DbContext
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure ViewModel classes as keyless
            modelBuilder.Entity<VendorFeedbackSummary>(entity =>
            {
                entity.HasNoKey();
                entity.ToView(null); // Optional: if you want to explicitly state that it is not mapped to a table
            });

            modelBuilder.Entity<AnswerRecord>(entity =>
            {
                entity.HasNoKey();
                entity.ToView(null); // Optional: if you want to explicitly state that it is not mapped to a table
            });  
            modelBuilder.Entity<FeedbackRequest>(entity =>
            {
                entity.HasNoKey();
                entity.ToView(null); // Optional: if you want to explicitly state that it is not mapped to a table
            });  
            modelBuilder.Entity<BranchFeedbackPercentage>(entity =>
            {
                entity.HasNoKey();
                entity.ToView(null); // Optional: if you want to explicitly state that it is not mapped to a table
            });
        }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<VendorMaster> VendorMasters { get; set; }
        public DbSet<VendorDetail> VendorDetails { get; set; }
        public DbSet<Questions> Questions { get; set; }
        public DbSet<Branch> Branches { get; set; }
        public DbSet<FeedbackMaster> FeedbackMasters { get; set; }
        public DbSet<FeedbackDetail> FeedbackDetails { get; set; }
        public virtual DbSet<feedback.Models.GetVendorServiceDetails> GetVendorServiceDetails { get; set; } = default!;

        public virtual DbSet<feedback.Models.DetailByServiceRegion> DetailByServiceRegion { get; set; } = default!;

        public DbSet<Service_answer> Service_answer { get; set; }
        public DbSet<suggestion_question> suggestion_question { get; set; }
        public DbSet<suggestion_answer> suggestion_answer { get; set; }
        public virtual DbSet<PerformanceVM> PerformanceVM { get; set; }
        public virtual DbSet<VendorFeedbackSummary> VendorFeedbackSummary { get; set; }
        public virtual DbSet<AnswerRecord> AnswerRecord { get; set; }
        public virtual DbSet<FeedbackRecord> FeedbackRecord { get; set; }
        public virtual DbSet<FeedbackRequest> FeedbackRequest { get; set; }
        public virtual DbSet<BranchFeedbackPercentage> BranchFeedbackPercentage { get; set; }
    }
}
