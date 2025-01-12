using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NotificationService.Interface;
using NotificationService.Model;

namespace NotificationService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        private readonly IEmailService _emailService;
        public NotificationsController(IEmailService emailService)
        {
            _emailService = emailService;
        }
        [HttpPost("send")]
        public async Task<IActionResult> SendNotification([FromBody] NotificationRequest notificationRequest)
        {
            // Send Email
            if (!string.IsNullOrEmpty(notificationRequest.Email))
            {
                await _emailService.SendEmailNotification(notificationRequest.Email, notificationRequest.Message);
            }
            return Ok("Notification sent successfully.");
        }
        }
}
