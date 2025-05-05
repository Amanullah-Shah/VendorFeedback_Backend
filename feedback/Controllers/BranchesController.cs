using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using feedback.Data;
using feedback.Models;
using Microsoft.AspNetCore.Cors;

namespace feedback.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class BranchesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public BranchesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Branches
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Branch>>> GetBranches()
        {
            return await _context.Branches.ToListAsync();
        }




        [HttpGet("PendingBranches")]
        public async Task<IActionResult> GetPendingBranches()
        {
            var branchesWithoutFeedback = await (from b in _context.Branches
                                                 join f in _context.FeedbackMasters on b.Id equals f.BranchId into feedbackGroup
                                                 from fg in feedbackGroup.DefaultIfEmpty()
                                                 group fg by new { b.Id, b.BranchName } into g
                                                 where g.Count(x => x != null) == 0 // No feedback records
                                                 select new
                                                 {
                                                     BranchId = g.Key.Id,
                                                     BranchName = g.Key.BranchName,
                                                     HasVendor1 = 0,
                                                     HasVendor2 = 0,
                                                     HasVendor3 = 0,
                                                     HasVendor4 = 0,
                                                     HasVendor5 = 0,
                                                     HasVendor6 = 0,
                                                     HasVendor7 = 0
                                                 }).ToListAsync();

            // Query 2: Branches with specific vendor conditions
            var branchesWithVendorConditions = await (from b in _context.Branches
                                                      join f in _context.FeedbackMasters on b.Id equals f.BranchId into feedbackGroup
                                                      from fg in feedbackGroup.DefaultIfEmpty()
                                                      group fg by new { b.Id, b.BranchName } into g
                                                      select new
                                                      {
                                                          BranchId = g.Key.Id,
                                                          BranchName = g.Key.BranchName,
                                                          HasVendor1 = g.Max(x => x != null && x.VendorMasterId == 1 ? 1 : 0),
                                                          HasVendor2 = g.Max(x => x != null && x.VendorMasterId == 2 ? 1 : 0),
                                                          HasVendor3 = g.Max(x => x != null && x.VendorMasterId == 3 ? 1 : 0),
                                                          HasVendor4 = g.Max(x => x != null && x.VendorMasterId == 7 ? 1 : 0),
                                                          HasVendor5 = g.Max(x => x != null && x.VendorMasterId == 8 ? 1 : 0),
                                                          HasVendor6 = g.Max(x => x != null && x.VendorMasterId == 9 ? 1 : 0),
                                                          HasVendor7 = g.Max(x => x != null && x.VendorMasterId == 11 ? 1 : 0),
                                                      })
                                                      .Where(x =>
    // Case 1: Only Vendor 1
    (x.HasVendor1 == 1 && x.HasVendor2 == 0 && x.HasVendor3 == 0) ||

    // Case 2: Only Vendor 2
    (x.HasVendor1 == 0 && x.HasVendor2 == 1 && x.HasVendor3 == 0) ||

    // Case 3: Only Vendor 3
    (x.HasVendor1 == 0 && x.HasVendor2 == 0 && x.HasVendor3 == 1) ||

    // Case 4: Vendor 1 and 2 only
    (x.HasVendor1 == 1 && x.HasVendor2 == 1 && x.HasVendor3 == 0) ||

    // Case 5: Vendor 1 and 3 only
    (x.HasVendor1 == 1 && x.HasVendor2 == 0 && x.HasVendor3 == 1) ||

    // Case 6: Vendor 2 and 3 only
    (x.HasVendor1 == 0 && x.HasVendor2 == 1 && x.HasVendor3 == 1) ||

    // Case 7: All three vendors
    (x.HasVendor1 == 1 && x.HasVendor2 == 1 && x.HasVendor3 == 1) ||

    // Case 8: No vendors
    (x.HasVendor1 == 0 && x.HasVendor2 == 0 && x.HasVendor3 == 0)
)

                                                       .ToListAsync();

            // Combine both results
            var combinedResult = new
            {
                BranchesWithoutFeedback = branchesWithoutFeedback,
                BranchesWithVendorConditions = branchesWithVendorConditions
            };

            return Ok(combinedResult);
        }

        // GET: api/Branches/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Branch>> GetBranch(int id)
        {
            var branch = await _context.Branches.FindAsync(id);

            if (branch == null)
            {
                return NotFound();
            }

            return branch;
        }

        // PUT: api/Branches/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBranch(int id, Branch branch)
        {
            if (id != branch.Id)
            {
                return BadRequest();
            }

            _context.Entry(branch).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BranchExists(id))
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

        // POST: api/Branches
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Branch>> PostBranch(Branch branch)
        {
            _context.Branches.Add(branch);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBranch", new { id = branch.Id }, branch);
        }

        // DELETE: api/Branches/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBranch(int id)
        {
            var branch = await _context.Branches.FindAsync(id);
            if (branch == null)
            {
                return NotFound();
            }

            _context.Branches.Remove(branch);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BranchExists(int id)
        {
            return _context.Branches.Any(e => e.Id == id);
        }
    }
}
