using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace feedback.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogController : ControllerBase
    {
        private static readonly string logFilePath = Path.Combine(Directory.GetCurrentDirectory(), "logs", "VendorFeedback-log.txt");

        [HttpPost("create-log")]
        public IActionResult CreateLog([FromBody] LogMessageDto logMessage)
        {
            try
            {
                // Ensure log directory exists
                var logDir = Path.Combine(Directory.GetCurrentDirectory(), "logs");
                if (!Directory.Exists(logDir))
                {
                    Directory.CreateDirectory(logDir);
                }

                // Write the log to the file with userId, timestamp, and message
                using (StreamWriter writer = new StreamWriter(logFilePath, true))
                {
                    writer.WriteLine($"{DateTime.Now}  - UserId: {logMessage.UserId} - Message: {logMessage.Message} - Timestamp: {logMessage.Timestamp}");
                }

                return Ok(new { message = "Log created successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error writing log: {ex.Message}" });
            }
        }
    }

    // DTO for log message
    public class LogMessageDto
    {
        public string UserId { get; set; }  // Include UserId for logging

        public string Message { get; set; }
       
        public string Timestamp { get; set; }  // Timestamp of the log event
    }
}