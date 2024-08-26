using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using feedback.Data;
using feedback.Models;

namespace feedback.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GetVendorServiceDetailsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public GetVendorServiceDetailsController(AppDbContext context)
        {
            _context = context;
        }
            
        // GET: api/GetVendorServiceDetails
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetVendorServiceDetails>>> GetGetVendorServiceDetails(int regionID)
        {

            return Ok(await 
                _context.VendorDetails
                .Where(x=>x.RegionId == regionID)
                .Include(x => x.VendorMaster)
                .Include(x => x.Service)
                .Select(x => new { x.ServiceId, x.Service.ServiceName, x.Service.icon, x.VendorMaster.Name })
                .ToListAsync());
            
            
            
            
        }
        [HttpGet("{sId}")]
        public async Task<ActionResult<object>> GetQuestionsAndSuggestionsBySId(int sId)
        {
            // Fetch the questions with the given S_id
            var questions = await _context.Questions
                .Where(q => q.S_id == sId)
                .ToListAsync();

            if (questions == null || !questions.Any())
            {
                return NotFound(new { message = "No questions found for the given S_id" });
            }

            // Create a list to store the question results
            var questionResults = new List<object>();

            foreach (var question in questions)
            {
                // Fetch the related answers for each question
                var answers = await _context.Service_answer
                    .Where(a => a.QuestionsId == question.Id)
                    .ToListAsync();

                // Prepare the result for this question
                var questionResult = new
                {
                    QuestionId = question.Id,
                    QuestionName = question.Name,
                    Answers = answers.ToDictionary(a => a.id, a => a.name)
                };

                questionResults.Add(questionResult);
            }

            // Fetch the suggestion questions with the given S_id
            var suggestionQuestions = await _context.suggestion_question
                .Where(sq => sq.s_id == sId)
                .ToListAsync();

            if (suggestionQuestions == null || !suggestionQuestions.Any())
            {
                return NotFound(new { message = "No suggestions found for the given S_id" });
            }

            // Create a list to store the suggestion results
            var suggestionResults = new List<object>();

            foreach (var suggestion in suggestionQuestions)
            {
                // Fetch the related answers for each suggestion question
                var suggestionAnswers = await _context.suggestion_answer
                    .Where(sa => sa.suggestionId == suggestion.id)
                    .ToListAsync();

                // Prepare the result for this suggestion question
                var suggestionResult = new
                {
                    SuggestionId = suggestion.id,
                    SuggestionName = suggestion.name,
                    Answers = suggestionAnswers.ToDictionary(sa => sa.id, sa => sa.name)
                };

                suggestionResults.Add(suggestionResult);
            }

            // Prepare the final result
            var result = new
            {
                Questions = questionResults,
                Suggestions = suggestionResults
            };

            return Ok(result);
        }


        [HttpGet("ByRegionAndServiceId")]
        public async Task<ActionResult<IEnumerable<DetailByServiceRegion>>> GetDetailByServiceRegion(int regionId, int serviceId)
        {
            var details = await _context.VendorDetails
                .Where(vd => vd.RegionId == regionId && vd.ServiceId == serviceId)
                .Include(vd => vd.Service)
                .Include(vd => vd.VendorMaster)
                .Select(vd => new DetailByServiceRegion
                {
                    ServiceId = vd.ServiceId,
                    ServiceName = vd.Service.ServiceName,
                    VendorName = vd.VendorMaster.Name,
                    From_Date = vd.Service.From_Date,
                    To_Date = vd.Service.To_Date,
                    Vendorid = vd.VendorMaster.Id
                    
                })
                .ToListAsync();

            return Ok(details);
        }

        // GET: api/GetVendorServiceDetails/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<GetVendorServiceDetails>> GetGetVendorServiceDetails(int id)
        //{
        //    var getVendorServiceDetails = await _context.GetVendorServiceDetails.FindAsync(id);

        //    if (getVendorServiceDetails == null)
        //    {
        //        return NotFound();
        //    }

        //    return getVendorServiceDetails;
        //}

        // PUT: api/GetVendorServiceDetails/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGetVendorServiceDetails(int id, GetVendorServiceDetails getVendorServiceDetails)
        {
            if (id != getVendorServiceDetails.ServiceId)
            {
                return BadRequest();
            }

            _context.Entry(getVendorServiceDetails).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GetVendorServiceDetailsExists(id))
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

        // POST: api/GetVendorServiceDetails
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<GetVendorServiceDetails>> PostGetVendorServiceDetails(GetVendorServiceDetails getVendorServiceDetails)
        {
            _context.GetVendorServiceDetails.Add(getVendorServiceDetails);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetGetVendorServiceDetails", new { id = getVendorServiceDetails.ServiceId }, getVendorServiceDetails);
        }

        // DELETE: api/GetVendorServiceDetails/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGetVendorServiceDetails(int id)
        {
            var getVendorServiceDetails = await _context.GetVendorServiceDetails.FindAsync(id);
            if (getVendorServiceDetails == null)
            {
                return NotFound();
            }

            _context.GetVendorServiceDetails.Remove(getVendorServiceDetails);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool GetVendorServiceDetailsExists(int id)
        {
            return _context.GetVendorServiceDetails.Any(e => e.ServiceId == id);
        }
    }
}
