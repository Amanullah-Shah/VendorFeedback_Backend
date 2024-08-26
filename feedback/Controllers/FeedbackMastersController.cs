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

namespace feedback.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeedbackMastersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public FeedbackMastersController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/FeedbackMasters
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FeedbackMaster>>> GetFeedbackMasters()
        {
            return await _context.FeedbackMasters.ToListAsync();
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
        private readonly string connectionString = "Data Source=192.168.1.9;Initial Catalog=feedback_react;Persist Security Info=True;User ID=sa;Password=Windows@3210;Trust Server Certificate=True";
        [HttpGet("history/{userId}/{vendorid}")]
        public async Task<IActionResult> GetFeedback(int userId, int vendorid)
        {
            var feedbacks = new Dictionary<int, FeedbackVM>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();
                using (SqlCommand cmd = new SqlCommand(@"
            SELECT 
                fm.Id AS FeedbackId,
                vm.Name AS VendorName,
                fm.Review,
                fm.Date,
                fm.Comment,
                q.Name AS QuestionName,
                sa.Name AS AnswerName,
                sq.Name AS SuggestionQuestionName,
                ssa.Name AS SuggestionAnswerName
            FROM 
                [feedback_react].[dbo].[FeedbackMasters] fm
            INNER JOIN 
                [feedback_react].[dbo].[VendorMasters] vm ON fm.VendorMasterId = vm.Id
            INNER JOIN 
                [feedback_react].[dbo].[FeedbackDetails] fd ON fm.Id = fd.FeedbackMasterId
            LEFT JOIN 
                [feedback_react].[dbo].[Questions] q ON fd.QuestionId = q.Id
            LEFT JOIN 
                [feedback_react].[dbo].[Service_answer] sa ON fd.AnswerID = sa.Id
            LEFT JOIN 
                [feedback_react].[dbo].[suggestion_question] sq ON fd.suggestionID = sq.Id
            LEFT JOIN 
                [feedback_react].[dbo].[suggestion_answer] ssa ON fd.suggestion_answerID = ssa.Id
            WHERE 
                fm.UserId = @UserId AND fm.VendorMasterId = @vendorid", conn))
                {
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    cmd.Parameters.AddWithValue("@vendorid", vendorid);
                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            int feedbackId = reader.GetInt32(reader.GetOrdinal("FeedbackId"));

                            if (!feedbacks.ContainsKey(feedbackId))
                            {
                                feedbacks[feedbackId] = new FeedbackVM
                                {
                                    FeedbackId = feedbackId,
                                    VendorName = reader.GetString(reader.GetOrdinal("VendorName")),
                                    Review = reader.GetInt32(reader.GetOrdinal("Review")),
                                    Date = reader.GetDateTime(reader.GetOrdinal("Date")),
                                    Comment = reader.GetString(reader.GetOrdinal("Comment")),
                                    Questions = new List<QuestionAnswer>(),
                                    Suggestions = new List<SuggestionAnswer>(),
                                    QuestionNames = new HashSet<string>(),
                                    SuggestionQuestionNames = new HashSet<string>()
                                };
                            }

                            var questionName = reader.IsDBNull(reader.GetOrdinal("QuestionName")) ? null : reader.GetString(reader.GetOrdinal("QuestionName"));
                            var answerName = reader.IsDBNull(reader.GetOrdinal("AnswerName")) ? null : reader.GetString(reader.GetOrdinal("AnswerName"));
                            var suggestionQuestionName = reader.IsDBNull(reader.GetOrdinal("SuggestionQuestionName")) ? null : reader.GetString(reader.GetOrdinal("SuggestionQuestionName"));
                            var suggestionAnswerName = reader.IsDBNull(reader.GetOrdinal("SuggestionAnswerName")) ? null : reader.GetString(reader.GetOrdinal("SuggestionAnswerName"));

                            if (!string.IsNullOrEmpty(questionName) && feedbacks[feedbackId].QuestionNames.Add(questionName))
                            {
                                feedbacks[feedbackId].Questions.Add(new QuestionAnswer
                                {
                                    QuestionName = questionName,
                                    AnswerName = answerName
                                });
                            }

                            if (!string.IsNullOrEmpty(suggestionQuestionName) && feedbacks[feedbackId].SuggestionQuestionNames.Add(suggestionQuestionName))
                            {
                                feedbacks[feedbackId].Suggestions.Add(new SuggestionAnswer
                                {
                                    SuggestionQuestionName = suggestionQuestionName,
                                    SuggestionAnswerName = suggestionAnswerName
                                });
                            }
                        }
                    }
                }
            }

            return Ok(feedbacks.Values.ToList());
        }


        private bool FeedbackMasterExists(int id)
        {
            return _context.FeedbackMasters.Any(e => e.Id == id);
        }
    }
}
