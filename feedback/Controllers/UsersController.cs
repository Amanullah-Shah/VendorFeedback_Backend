using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using feedback.Data;
using feedback.Models;

namespace feedback.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILogger<UsersController> _logger;

        public UsersController(ILogger<UsersController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _context.Users.Where(x => x.Id == id).FirstOrDefaultAsync();

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        [HttpPost("Validate")]
        public async Task<ActionResult> ValidateUser([FromBody] UserLoginDto loginDto)
        {
            // Log that login attempt is being made
            _logger.LogInformation($"Login attempt for User: {loginDto.LoginId}");

            var user = await _context.Users
                .Where(u => u.ErpId == loginDto.LoginId && u.Password == loginDto.Password)
                .Select(u => new
                {
                    isValid = true,
                    RegId = u.RegionId,
                    name = u.Name,
                    branch = u.BranchId,
                    erp = u.ErpId,
                    id = u.Id,
                    role = u.Role
                })
                .FirstOrDefaultAsync();

            if (user != null)
            {
                // Log successful login
                _logger.LogInformation($"User {user.erp} (ID: {user.id}) successfully logged in. Region: {user.RegId}, Branch: {user.branch}");
                return Ok(user);
            }

            // Log failed login attempt
            _logger.LogWarning($"Login failed for User: {loginDto.LoginId}");

            return Ok(new { isValid = false });
        }


        // PUT: api/Users/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
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

        // POST: api/Users
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = user.Id }, user);
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }

    public class UserLoginDto
    {
        public string? LoginId { get; set; }
        public string? Password { get; set; }
    }
}
