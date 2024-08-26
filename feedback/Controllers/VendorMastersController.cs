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

namespace feedback.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VendorMastersController : ControllerBase
    {
        private readonly string connectionString = "Data Source=192.168.1.9;Initial Catalog=feedback_react;Persist Security Info=True;User ID=sa;Password=Windows@3210;Trust Server Certificate=True";

        private readonly AppDbContext _context;

        public VendorMastersController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/VendorMasters
        [HttpGet]
        public async Task<ActionResult<IEnumerable<VendorMaster>>> GetVendorMasters()
        {
            return await _context.VendorMasters.ToListAsync();
        }

        // GET: api/VendorMasters/5
        [HttpGet("{vendorMasterId}")]
        public IActionResult GetQuestionsByVendorMasterId(int vendorMasterId)
        {
            var result = (from q in _context.Questions
                          join v in _context.VendorDetails
                          on q.S_id equals v.ServiceId
                          where v.VendorMasterId == vendorMasterId
                          group new { q.Id, q.Name, q.S_id } by q.Id into g
                          select new QuestionViewModel
                          {
                              QuestionId = g.Key,
                              QuestionName = g.FirstOrDefault().Name,
                              ServiceId = g.FirstOrDefault().S_id
                          }).ToList();

            return Ok(result);
        }

        // PUT: api/VendorMasters/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutVendorMaster(int id, VendorMaster vendorMaster)
        {
            if (id != vendorMaster.Id)
            {
                return BadRequest();
            }

            _context.Entry(vendorMaster).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VendorMasterExists(id))
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

        // POST: api/VendorMasters
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<VendorMaster>> PostVendorMaster(VendorMaster vendorMaster)
        {
            _context.VendorMasters.Add(vendorMaster);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetVendorMaster", new { id = vendorMaster.Id }, vendorMaster);
        }

        // DELETE: api/VendorMasters/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVendorMaster(int id)
        {
            var vendorMaster = await _context.VendorMasters.FindAsync(id);
            if (vendorMaster == null)
            {
                return NotFound();
            }

            _context.VendorMasters.Remove(vendorMaster);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool VendorMasterExists(int id)
        {
            return _context.VendorMasters.Any(e => e.Id == id);
        }
    }
}
