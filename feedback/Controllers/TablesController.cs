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

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace feedback.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TablesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TablesController(AppDbContext context)
        {
            _context = context;
        }
        // GET: api/<TablesController>

        [HttpGet("users")]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        // Endpoint to get all Regions
        [HttpGet("regions")]
        public async Task<ActionResult<IEnumerable<Region>>> GetRegions()
        {
            return await _context.Regions.ToListAsync();
        }

        // Endpoint to get all Services
        [HttpGet("services")]
        public async Task<ActionResult<IEnumerable<Service>>> GetServices()
        {
            return await _context.Services.ToListAsync();
        }

        // Endpoint to get all VendorMasters
        [HttpGet("vendormasters")]
        public async Task<ActionResult<IEnumerable<VendorMaster>>> GetVendorMasters()
        {
            return await _context.VendorMasters.ToListAsync();
        }

        // Endpoint to get all VendorDetails
        [HttpGet("vendordetails")]
        public async Task<ActionResult<IEnumerable<VendorDetail>>> GetVendorDetails()
        {
            return await _context.VendorDetails.ToListAsync();
        }

        // Endpoint to get all Questions
        [HttpGet("questions")]
        public async Task<ActionResult<IEnumerable<Questions>>> GetQuestions()
        {
            return await _context.Questions.ToListAsync();
        }

        // Endpoint to get all Branches
        [HttpGet("branches")]
        public async Task<ActionResult<IEnumerable<Branch>>> GetBranches()
        {
            return await _context.Branches.ToListAsync();
        }

        // Endpoint to get all FeedbackMasters
        [HttpGet("feedbackmasters")]
        public async Task<ActionResult<IEnumerable<FeedbackMaster>>> GetFeedbackMasters()
        {
            return await _context.FeedbackMasters.ToListAsync();
        }

        // Endpoint to get all FeedbackDetails
        [HttpGet("feedbackdetails")]
        public async Task<ActionResult<IEnumerable<FeedbackDetail>>> GetFeedbackDetails()
        {
            return await _context.FeedbackDetails.ToListAsync();
        }

        // Endpoint to get all Service_answers
        [HttpGet("service_answers")]
        public async Task<ActionResult<IEnumerable<Service_answer>>> GetServiceAnswers()
        {
            return await _context.Service_answer.ToListAsync();
        }

        // Endpoint to get all suggestion_questions
        [HttpGet("suggestion_questions")]
        public async Task<ActionResult<IEnumerable<suggestion_question>>> GetSuggestionQuestions()
        {
            return await _context.suggestion_question.ToListAsync();
        }

        // Endpoint to get all suggestion_answers
        [HttpGet("suggestion_answers")]
        public async Task<ActionResult<IEnumerable<suggestion_answer>>> GetSuggestionAnswers()
        {
            return await _context.suggestion_answer.ToListAsync();
        }


    }
}
