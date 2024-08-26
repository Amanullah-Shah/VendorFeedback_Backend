using feedback.Data;
using feedback.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace feedback.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DashboardController(AppDbContext context)
        {
            _context = context;
        }




        [HttpGet("piechart/{questionId}")]
        public async Task<IActionResult> GetChartSummary(int questionId)
        {
            // Retrieve AnswerID for the given QuestionId
            var answerIds = await _context.FeedbackDetails
                .Where(fd => fd.QuestionId == questionId)
                .Select(fd => fd.AnswerID)
                .Distinct()
                .ToListAsync();

            // Get answer details for the question
            var answers = await _context.Service_answer
                .Where(a => a.QuestionsId == questionId)
                .ToListAsync();

            // Retrieve feedback counts based on answer IDs
            var feedbackCounts = await _context.FeedbackDetails
                .Where(fd => fd.QuestionId == questionId)
                .GroupBy(fd => fd.AnswerID)
                .Select(g => new
                {
                    AnswerID = g.Key,
                    Count = g.Count()
                })
                .ToListAsync();

            // Prepare the answer summary
            var totalCount = feedbackCounts.Sum(fc => fc.Count);
            var answerSummaries = answers.Select(a =>
            {
                var count = feedbackCounts.FirstOrDefault(fc => fc.AnswerID == a.id)?.Count ?? 0;
                var percentage = totalCount > 0 ? Math.Round((count / (double)totalCount) * 100, 2) : 0;

                return new AnswerSummary
                {
                    Name = a.name,
                    Count = count,
                    Percentage = percentage
                };
            }).ToList();

            return Ok(answerSummaries);
        }
        [HttpGet("summary/{date}")]
        public async Task<IActionResult> GetFeedbackSummary(DateTime date)
        {
            var endDate = DateTime.Today;
            var feedbackDetails = await _context.PerformanceVM
              .FromSqlRaw(@"
                SELECT  d.[Id]
     
      ,d.[QuestionId]
      ,d.[AnswerID],
	  v.Name,
	  v.Id as vendorid


   
  FROM [feedback_react].[dbo].[FeedbackDetails] d 
  inner join [feedback_react].[dbo].[FeedbackMasters] f on f.Id = d.FeedbackMasterId
  inner join [feedback_react].[dbo].VendorMasters v on v.Id = f.VendorMasterId where f.Date BETWEEN {0} AND {1}
            ", date, endDate)
              .ToListAsync();

            // Group by Vendor
            var groupedByVendor = feedbackDetails.GroupBy(fd => new { fd.vendorid, fd.Name });

            var vendorSummaries = groupedByVendor.Select(group =>
            {
                var totalCount = group.Count();
                var poorCount = group.Count(fd => IsPoor(fd.AnswerID));
                var normalCount = group.Count(fd => IsNormal(fd.AnswerID));
                var goodCount = group.Count(fd => IsGood(fd.AnswerID));

                var poorPercentage = totalCount > 0 ? Math.Round((poorCount / (double)totalCount) * 100, 2) : 0;
                var normalPercentage = totalCount > 0 ? Math.Round((normalCount / (double)totalCount) * 100, 2) : 0;
                var goodPercentage = totalCount > 0 ? Math.Round((goodCount / (double)totalCount) * 100, 2) : 0;

                return new FeedbackSummary
                {
                    VendorId = group.Key.vendorid,
                    VendorName = group.Key.Name,
                    TotalCount = totalCount,
                    PoorCount = poorCount,
                    NormalCount = normalCount,
                    GoodCount = goodCount,
                    PoorPercentage = poorPercentage,
                    NormalPercentage = normalPercentage,
                    GoodPercentage = goodPercentage
                };
            }).ToList();

            return Ok(vendorSummaries);
        }



        [HttpGet("summary")]
        public async Task<IActionResult> GetSummary()
        {
            // Raw SQL Query
            var feedbackDetails = await _context.PerformanceVM
                .FromSqlRaw(@"
                SELECT 
                    d.[Id], 
                    d.[QuestionId], 
                    d.[AnswerID], 
                    v.Name, 
                    v.Id as vendorId
                FROM [feedback_react].[dbo].[FeedbackDetails] d 
                INNER JOIN [feedback_react].[dbo].[FeedbackMasters] f ON f.Id = d.FeedbackMasterId
                INNER JOIN [feedback_react].[dbo].[VendorMasters] v ON v.Id = f.VendorMasterId
            ")
                .ToListAsync();

            // Group by Vendor
            var groupedByVendor = feedbackDetails.GroupBy(fd => new { fd.vendorid, fd.Name });

            var vendorSummaries = groupedByVendor.Select(group =>
            {
                var totalCount = group.Count();
                var poorCount = group.Count(fd => IsPoor(fd.AnswerID));
                var normalCount = group.Count(fd => IsNormal(fd.AnswerID));
                var goodCount = group.Count(fd => IsGood(fd.AnswerID));

                var poorPercentage = totalCount > 0 ? Math.Round((poorCount / (double)totalCount) * 100, 2) : 0;
                var normalPercentage = totalCount > 0 ? Math.Round((normalCount / (double)totalCount) * 100, 2) : 0;
                var goodPercentage = totalCount > 0 ? Math.Round((goodCount / (double)totalCount) * 100, 2) : 0;

                return new FeedbackSummary
                {
                    VendorId = group.Key.vendorid,
                    VendorName = group.Key.Name,
                    TotalCount = totalCount,
                    PoorCount = poorCount,
                    NormalCount = normalCount,
                    GoodCount = goodCount,
                    PoorPercentage = poorPercentage,
                    NormalPercentage = normalPercentage,
                    GoodPercentage = goodPercentage
                };
            }).ToList();

            return Ok(vendorSummaries);
        }


        // Determines if the AnswerID is classified as poor
        private bool IsPoor(int answerId)
        {
            // IDs 1, 4, 7, 10, ... are poor
            return (answerId - 1) % 3 == 0;
        }

        // Determines if the AnswerID is classified as normal
        private bool IsNormal(int answerId)
        {
            // IDs 2, 5, 8, 11, ... are normal
            return (answerId - 2) % 3 == 0;
        }

        // Determines if the AnswerID is classified as good
        private bool IsGood(int answerId)
        {
            // IDs 3, 6, 9, 12, ... are good
            return (answerId - 3) % 3 == 0;
        }


        [HttpGet("feedback-summary")]
        public async Task<IActionResult> GetFeedbackSummary()
        {
            var feedbackDetails = await _context.FeedbackRecord
      .FromSqlRaw(@"
            SELECT 
                d.[Id], 
                d.[QuestionId], 
                d.[AnswerID] AS AnswerId, 
                s.Name AS AnswerName, 
                v.Name AS VendorName, 
                v.Id AS VendorId
            FROM [feedback_react].[dbo].[FeedbackDetails] d
            INNER JOIN [feedback_react].[dbo].[FeedbackMasters] f ON f.Id = d.FeedbackMasterId
            INNER JOIN [feedback_react].[dbo].[VendorMasters] v ON v.Id = f.VendorMasterId
            INNER JOIN [feedback_react].[dbo].[Service_answer] s ON s.Id = d.AnswerID
        ")
      .ToListAsync();

            // Group by Vendor and Question
            var groupedByVendorAndQuestion = feedbackDetails
                .GroupBy(fd => new { fd.VendorId, fd.VendorName, fd.QuestionId });

            // Create the summary list
            var vendorFeedbackSummaries = groupedByVendorAndQuestion.Select(group =>
            {
                var totalCount = group.Count();
                var answersGrouped = group.GroupBy(g => new { g.AnswerId, g.AnswerName })
                                          .Select(ag => new AnswerRecord
                                          {
                                              AnswerId = ag.Key.AnswerId,
                                              AnswerName = ag.Key.AnswerName,
                                              ResponseCount = totalCount > 0 ? Math.Round((ag.Count() / (double)totalCount) * 100, 2) : 0

                                          }).ToList();

                return new VendorFeedbackSummary
                {
                    VendorId = group.Key.VendorId,
                    VendorName = group.Key.VendorName,
                    QuestionId = group.Key.QuestionId,
                    AnswerRecord = answersGrouped
                };
            }).ToList();


            return Ok(vendorFeedbackSummaries);
        }




        [HttpPost("calculate-percentage")]
        public IActionResult CalculateFeedbackPercentage([FromBody] FeedbackRequest request)
        {
            if (request == null || request.AnswerIds == null || !request.AnswerIds.Any())
            {
                return BadRequest("Invalid request. Vendor ID and Answer IDs cannot be null or empty.");
            }

            var feedbackMasters = _context.FeedbackMasters
                .Where(fm => fm.VendorMasterId == request.VendorId)
                .ToList();

            var feedbackMasterIds = feedbackMasters.Select(fm => fm.Id).ToList();

            // Fetch all feedback details for the given vendor
            var allFeedbackDetails = _context.FeedbackDetails
                .Where(fd => feedbackMasterIds.Contains(fd.FeedbackMasterId))
                .ToList();

            // Fetch feedback details for the given answer IDs
            var filteredFeedbackDetails = allFeedbackDetails
                .Where(fd => request.AnswerIds.Contains(fd.AnswerID))
                .ToList();

            var totalFeedbackCount = allFeedbackDetails.Count;
            var filteredFeedbackCount = filteredFeedbackDetails.Count;

            // Calculate percentage for each branch
            var result = new List<BranchFeedbackPercentage>();

            foreach (var branchGroup in feedbackMasters.GroupBy(fm => fm.BranchId))
            {
                var branchId = branchGroup.Key;
                var branch = _context.Branches.FirstOrDefault(b => b.Id == branchId);

                if (branch == null)
                {
                    continue; // Skip if the branch is not found
                }

                var branchName = branch.BranchName;

                var branchFeedbackCount = branchGroup.Count();
                var branchFilteredFeedbackCount = filteredFeedbackDetails
                    .Count(fd => branchGroup.Any(fm => fm.Id == fd.FeedbackMasterId));

                var percentage = (filteredFeedbackCount > 0)
                    ? (double)branchFilteredFeedbackCount / filteredFeedbackCount * 100
                    : 0;

                result.Add(new BranchFeedbackPercentage
                {
                    Id = branch.Id,
                    BranchName = branchName,
                    Percentage = percentage
                });
            }

            return Ok(result);
        }


    }
}
