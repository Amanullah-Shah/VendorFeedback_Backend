using feedback.Data;
using feedback.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

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




        [HttpGet("piechart/{questionId}/{regionId}")]
        public async Task<IActionResult> GetChartSummary(int questionId, int regionId)
        {
            if (regionId != 0)
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
                    .Where(fd => fd.QuestionId == questionId &&
                    _context.FeedbackMasters.Any(fm => fm.Id == fd.FeedbackMasterId && fm.RegionId == regionId)
                    )
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
            else
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
                    .Where(fd => fd.QuestionId == questionId &&
                    _context.FeedbackMasters.Any(fm => fm.Id == fd.FeedbackMasterId)
                    )
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
        }


        [HttpGet("piechartBranchDetails/{questionId}/{regionId}/{answerName}")]
        public async Task<IActionResult> GetChartSummaryBranch(int questionId, int regionId, string answerName)
        {
            if(regionId!=0)
            {
                // Retrieve AnswerID for the given QuestionId

                var answerID = await _context.Service_answer
                    .Where(sa => sa.name == answerName && sa.QuestionsId == questionId)
                    .Select(sa => sa.id)
                    .FirstOrDefaultAsync();
                // Retrieve feedback counts based on answer IDs
                var feedbackCounts = await _context.FeedbackDetails
           .Where(fd => fd.QuestionId == questionId && fd.AnswerID == answerID &&
           _context.FeedbackMasters.Any(fm => fm.Id == fd.FeedbackMasterId && fm.RegionId == regionId)
           )
           .Select(fd => new
           {
               BranchId = _context.FeedbackMasters
                   .Where(fm => fm.Id == fd.FeedbackMasterId)
                   .Select(fm => fm.BranchId)
                   .FirstOrDefault(),

               Comment = _context.FeedbackMasters
                   .Where(fm => fm.Id == fd.FeedbackMasterId)
                   .Select(fm => fm.Comment)
                   .FirstOrDefault()  // Get the Review column from the FeedbackMasters table
           })
           .Distinct()
           .ToListAsync();

                var feedbackWithBranchNames = feedbackCounts.Select(fc => new
                {
                    BranchName = _context.Branches
                        .Where(b => b.Id == fc.BranchId)
                        .Select(b => b.BranchName)
                        .FirstOrDefault(),  // Get BranchName from the Branches table

                    Comment = fc.Comment  // Include the Review column in the result
                }).ToList();

                return Ok(feedbackWithBranchNames);
            }
            else
            {
                // Retrieve AnswerID for the given QuestionId

                var answerID = await _context.Service_answer
                    .Where(sa => sa.name == answerName && sa.QuestionsId == questionId)
                    .Select(sa => sa.id)
                    .FirstOrDefaultAsync();
                // Retrieve feedback counts based on answer IDs
                var feedbackCounts = await _context.FeedbackDetails
           .Where(fd => fd.QuestionId == questionId && fd.AnswerID == answerID &&
           _context.FeedbackMasters.Any(fm => fm.Id == fd.FeedbackMasterId)
           )
           .Select(fd => new
           {
               BranchId = _context.FeedbackMasters
                   .Where(fm => fm.Id == fd.FeedbackMasterId)
                   .Select(fm => fm.BranchId)
                   .FirstOrDefault(),

               Comment = _context.FeedbackMasters
                   .Where(fm => fm.Id == fd.FeedbackMasterId)
                   .Select(fm => fm.Comment)
                   .FirstOrDefault()  // Get the Review column from the FeedbackMasters table
           })
           .Distinct()
           .ToListAsync();

                var feedbackWithBranchNames = feedbackCounts.Select(fc => new
                {
                    BranchName = _context.Branches
                        .Where(b => b.Id == fc.BranchId)
                        .Select(b => b.BranchName)
                        .FirstOrDefault(),  // Get BranchName from the Branches table

                    Comment = fc.Comment  // Include the Review column in the result
                }).ToList();

                return Ok(feedbackWithBranchNames);
            }


          
        }



        [HttpGet("summary/{date}")]
        public async Task<IActionResult> GetFeedbackSummary(DateTime date)
        {
            var endDate = DateTime.Today;

            // Fetching data using LINQ query instead of raw SQL
            var feedbackDetails = (from d in _context.FeedbackDetails
                           join f in _context.FeedbackMasters on d.FeedbackMasterId equals f.Id
                           join v in _context.VendorMasters on f.VendorMasterId equals v.Id
                           where f.Date >= date && f.Date <= endDate
                           select new
                           {
                               FeedbackDetailId = d.Id,
                               QuestionId = d.QuestionId,
                               AnswerId = d.AnswerID,
                               VendorName = v.Name,
                               VendorId = v.Id
                           }).ToList();


            //var feedbackDetails = await _context.FeedbackDetails
            //    .Where(d => d.FeedbackMaster.Date >= date && d.FeedbackMaster.Date <= endDate)
            //    .Select(d => new PerformanceVM
            //    {
            //        Id = d.Id,
            //        QuestionId = d.QuestionId,
            //        AnswerID = d.AnswerID,
            //        vendorid = d.FeedbackMaster.VendorMaster.Id,
            //        Name = d.FeedbackMaster.VendorMaster.Name
            //    })
            //    .ToListAsync();

            // Group by Vendor
            var groupedByVendor = feedbackDetails.GroupBy(fd => new { fd.VendorId, fd.VendorName });

            var vendorSummaries = groupedByVendor.Select(group =>
            {
                var totalCount = group.Count();
                var poorCount = group.Count(fd => IsPoor(fd.AnswerId));
                var normalCount = group.Count(fd => IsNormal(fd.AnswerId));
                var goodCount = group.Count(fd => IsGood(fd.AnswerId));

                var poorPercentage = totalCount > 0 ? Math.Round((poorCount / (double)totalCount) * 100, 2) : 0;
                var normalPercentage = totalCount > 0 ? Math.Round((normalCount / (double)totalCount) * 100, 2) : 0;
                var goodPercentage = totalCount > 0 ? Math.Round((goodCount / (double)totalCount) * 100, 2) : 0;

                return new FeedbackSummary
                {
                    VendorId = group.Key.VendorId,
                    VendorName = group.Key.VendorName,
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


            var feedbackDetails = (from d in _context.FeedbackDetails
                                   join f in _context.FeedbackMasters on d.FeedbackMasterId equals f.Id
                                   join v in _context.VendorMasters on f.VendorMasterId equals v.Id
                                   select new
                                   {
                                       FeedbackDetailId = d.Id,
                                       QuestionId = d.QuestionId,
                                       AnswerId = d.AnswerID,
                                       VendorName = v.Name,
                                       VendorId = v.Id
                                   }).ToList();
            //var feedbackDetails = await _context.FeedbackDetails
            //    .Include(d => d.FeedbackMaster)       // Include FeedbackMaster to access related data
            //    .ThenInclude(fm => fm.VendorMaster)    // Include VendorMaster to access related data
            //    .Select(d => new PerformanceVM
            //    {
            //        Id = d.Id,
            //        QuestionId = d.QuestionId,
            //        AnswerID = d.AnswerID,
            //        Name = d.FeedbackMaster.VendorMaster.Name,
            //        vendorid = d.FeedbackMaster.VendorMaster.Id
            //    })
            //    .ToListAsync();

            // Group by Vendor
            var groupedByVendor = feedbackDetails.GroupBy(fd => new { fd.VendorId, fd.VendorName });

            var vendorSummaries = groupedByVendor.Select(group =>
            {
                var totalCount = group.Count();
                var poorCount = group.Count(fd => IsPoor(fd.AnswerId));
                var normalCount = group.Count(fd => IsNormal(fd.AnswerId));
                var goodCount = group.Count(fd => IsGood(fd.AnswerId));

                var poorPercentage = totalCount > 0 ? Math.Round((poorCount / (double)totalCount) * 100, 2) : 0;
                var normalPercentage = totalCount > 0 ? Math.Round((normalCount / (double)totalCount) * 100, 2) : 0;
                var goodPercentage = totalCount > 0 ? Math.Round((goodCount / (double)totalCount) * 100, 2) : 0;

                return new FeedbackSummary
                {
                    VendorId = group.Key.VendorId,
                    VendorName = group.Key.VendorName,
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
            try
            {
                // Fetch data with related entities using Includes

                var feedbackDetails = (from d in _context.FeedbackDetails
                              join f in _context.FeedbackMasters on d.FeedbackMasterId equals f.Id
                              join v in _context.VendorMasters on f.VendorMasterId equals v.Id
                              join s in _context.Service_answer on d.AnswerID equals s.id
                              select new
                              {
                                  Id = d.Id,
                                  QuestionId = d.QuestionId,
                                  AnswerId = d.AnswerID,
                                  AnswerName = s.name,
                                  VendorName = v.Name,
                                  VendorId = v.Id
                              }).ToList();
                //var feedbackDetails = await _context.FeedbackDetails
                //    .Include(d => d.FeedbackMaster)        // Include related FeedbackMaster entity
                //    .ThenInclude(fm => fm.VendorMaster)     // Include related VendorMaster entity from FeedbackMaster
                //    .Include(d => d.ServiceAnswer)         // Include related ServiceAnswer entity
                //    .Select(d => new FeedbackRecord
                //    {
                //        Id = d.Id,
                //        QuestionId = d.QuestionId,
                //        AnswerId = d.AnswerID,
                //        AnswerName = d.ServiceAnswer.name,
                //        VendorName = d.FeedbackMaster.VendorMaster.Name,
                //        VendorId = d.FeedbackMaster.VendorMaster.Id
                //    })
                //    .ToListAsync();

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
            catch (Exception ex)
            {
                // Log the exception and return a user-friendly message
               
                return StatusCode(500, "Internal server error. Please try again later.");
            }
        }



        [HttpGet("feedback-Detail_summary")]
        public async Task<IActionResult> GetFeedbackDetailSummary()
        {
            try
            {
                // Fetch data with related entities using Includes

                var feedbackData = await (from fm in _context.FeedbackMasters
                                          join b in _context.Branches on fm.BranchId equals b.Id
                                          join vm in _context.VendorMasters on fm.VendorMasterId equals vm.Id
                                          join fd in _context.FeedbackDetails on fm.Id equals fd.FeedbackMasterId
                                          join q in _context.Questions on fd.QuestionId equals q.Id
                                          join sa in _context.Service_answer on fd.AnswerID equals sa.id
                                          where q.Id == sa.QuestionsId
                                          orderby fm.Date descending
                                          select new
                                          {
                                              FeedbackMasterId = fm.Id,
                                              BranchName = b.BranchName,
                                              VendorName = vm.Name,
                                              Question = q.Id,
                                              Answer = sa.name,
                                              fm.Date,
                                              fm.Comment
                                          }).ToListAsync();

                var groupedFeedback = feedbackData
                    .GroupBy(f => new { f.FeedbackMasterId, f.BranchName })
                    .Select(g => new
                    {
                        FeedbackMasterId = g.Key.FeedbackMasterId,
                        BranchName = g.Key.BranchName,
                        Vendors = g.GroupBy(v => v.VendorName)
                                   .Select(v => new
                                   {
                                       VendorName = v.Key,
                                       QuestionsAndAnswers = v.Select(q => new
                                       {
                                           q.Question,
                                           q.Answer
                                       }).ToList()
                                   }).ToList(),
                       
                    });




                return Ok(groupedFeedback);
            
            }
            catch (Exception ex)
            {
                // Log the exception and return a user-friendly message

                return StatusCode(500, "Internal server error. Please try again later.");
            }
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
