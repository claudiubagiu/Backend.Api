using Notification.Api.Services.Interface;
using System.Net;
using System.Net.Mail;

namespace Notification.Api.Services.Implementation
{
    public class EmailService : IEmailService
    {
        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            var mail = "claudiu.bagiu03@gmail.com";
            var password = "yezj yosn ltdd qtcu";

            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(mail, password)
            };

            await client.SendMailAsync(new MailMessage
            {
                From = new MailAddress(mail),
                To = { new MailAddress(toEmail) },
                Subject = subject,
                Body = body,
                IsBodyHtml = true,
            });
        }
    }
}
