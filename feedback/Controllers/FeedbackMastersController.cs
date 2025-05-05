using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using feedback.Data;
using feedback.Models;
using feedback.ViewModel;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace feedback.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeedbackMastersController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly string? _connectionString;

        public FeedbackMastersController(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        // GET: api/FeedbackMasters
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FeedbackMaster>>> GetFeedbackMasters()
        {
            return await _context.FeedbackMasters.ToListAsync();
        }


        [HttpGet("OverallRating/{vendorId}/{regionId}")]
        public async Task<ActionResult<IEnumerable<FeedbackMaster>>> GetOverallRating(int vendorId,int regionId)
        {
           
            if (regionId!=0)
            {
                  var ratingdata = await _context.FeedbackMasters
                    .Where(x => x.VendorMasterId == vendorId && x.RegionId == regionId)
                    .Select(x => new
                    {
                        BranchName = _context.Branches.Where(b => b.Id == x.BranchId).Select(b => b.BranchName).FirstOrDefault(),
                        x.Review,
                        RegionName = _context.Regions.Where(r => r.Id == x.RegionId).Select(r => r.RegionName).FirstOrDefault(),
                        x.Date,
                        x.Comment
                    }).ToListAsync();
                return Ok(ratingdata);

            }
            else
            {
                   var ratingdata = await _context.FeedbackMasters
                  .Where(x => x.VendorMasterId == vendorId)
                  .Select(x => new
                  {
                      BranchName = _context.Branches.Where(b => b.Id == x.BranchId).Select(b => b.BranchName).FirstOrDefault(),
                      x.Review,
                      RegionName = _context.Regions.Where(r => r.Id == x.RegionId).Select(r => r.RegionName).FirstOrDefault(),
                      x.Date,
                      x.Comment
                  }).ToListAsync();
                return Ok(ratingdata);
            }
          
        }


        // GET: api/FeedbackMasters/5
        [HttpGet("{id}")]
        public async Task<ActionResult<FeedbackMaster>> GetFeedbackMaster(int id)
        {
            var feedbackMaster = await _context.FeedbackMasters.FindAsync(id);

            if (feedbackMaster == null)
            {
                return NotFound();
            }

            return feedbackMaster;
        }

        // PUT: api/FeedbackMasters/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFeedbackMaster(int id, FeedbackMaster feedbackMaster)
        {
            if (id != feedbackMaster.Id)
            {
                return BadRequest();
            }

            _context.Entry(feedbackMaster).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FeedbackMasterExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/   
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<FeedbackMaster>> PostFeedbackMaster(FeedbackMaster feedbackMaster)
        {
            _context.FeedbackMasters.Add(feedbackMaster);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFeedbackMaster", new { id = feedbackMaster.Id }, feedbackMaster);
        }

        // DELETE: api/FeedbackMasters/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFeedbackMaster(int id)
        {
            var feedbackMaster = await _context.FeedbackMasters.FindAsync(id);
            if (feedbackMaster == null)
            {
                return NotFound();
            }

            _context.FeedbackMasters.Remove(feedbackMaster);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        [HttpGet("history/{userId}/{vendorid}")]
        public async Task<IActionResult> GetFeedback(int userId, int vendorId)
        {
            // Fetch feedback details using LINQ with includes for related data
            var feedbackDetails = await _context.FeedbackMasters
                .Where(fm => fm.UserId == userId && fm.VendorMasterId == vendorId)
                .Include(fm => fm.VendorMaster)
                .Include(fm => fm.FeedbackDetails)
                    .ThenInclude(fd => fd.Question)         // Include related Questions
                .Include(fm => fm.FeedbackDetails)
                    .ThenInclude(fd => fd.ServiceAnswer)   // Include related ServiceAnswer
                .Include(fm => fm.FeedbackDetails)
                    .ThenInclude(fd => fd.SuggestionQuestion) // Include related SuggestionQuestion
                .Include(fm => fm.FeedbackDetails)
                    .ThenInclude(fd => fd.SuggestionAnswer)   // Include related SuggestionAnswer
                .Select(fm => new FeedbackVM
                {
                    FeedbackId = fm.Id,
                    VendorName = fm.VendorMaster.Name,
                    Review = fm.Review,
                    Date = fm.Date,
                    Comment = fm.Comment,
                    Questions = fm.FeedbackDetails
                        .Where(fd => fd.Question != null && fd.ServiceAnswer != null)
                        .GroupBy(fd => new { fd.Question.Name, fd.ServiceAnswer.name })
                        .Select(g => new QuestionAnswer
                        {
                            QuestionName = g.Key.Name,
                            AnswerName = g.Key.name
                        })
                        .ToList(),
                    Suggestions = fm.FeedbackDetails
                .Where(fd => fd.SuggestionQuestion != null)
                .GroupBy(fd => new { SuggestionQuestionName = fd.SuggestionQuestion.name, SuggestionAnswerName = fd.SuggestionAnswer.name })
                .Select(g => new SuggestionAnswer
                {
                    SuggestionQuestionName = g.Key.SuggestionQuestionName,
                    SuggestionAnswerName = g.Key.SuggestionAnswerName
                })
                .ToList()
                })
                .ToListAsync();

            return Ok(feedbackDetails);
        }




        private bool FeedbackMasterExists(int id)
        {
            return _context.FeedbackMasters.Any(e => e.Id == id);
        }
    }
}
