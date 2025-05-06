using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Notification.Api.Models;
using Notification.Api.Services.Implementation;
using Notification.Api.Services.Interface;
using Org.BouncyCastle.Asn1.Ocsp;

namespace Notification.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationController : ControllerBase
    {
        private readonly ISmsService smsService;
        private readonly IEmailService emailService;

        public NotificationController(ISmsService smsService, IEmailService emailService)
        {
            this.smsService = smsService;
            this.emailService = emailService;
        }

        [HttpPost("send-sms")]
        public async Task<IActionResult> SendSms([FromBody] SmsRequest smsRequest)
        {
            await smsService.SendSmsAsync(smsRequest.PhoneNumber, smsRequest.Message);
            return Ok("SMS sent!");
        }

        [HttpPost("send-email")]
        public async Task<IActionResult> SendEmail([FromBody] EmailRequest emailRequest)
        {
            await emailService.SendEmailAsync(emailRequest.ToEmail, emailRequest.Subject, emailRequest.Body);
            return Ok("Email sent!");
        }

        [HttpPost("send-notification")]
        public async Task<IActionResult> SendNotification([FromBody] NotificationRequest notificationRequest)
        {
            await smsService.SendSmsAsync(notificationRequest.PhoneNumber, notificationRequest.Message);
            await emailService.SendEmailAsync(notificationRequest.ToEmail, notificationRequest.Subject, notificationRequest.Body);
            return Ok("Notification sent!");
        }
    }
}
