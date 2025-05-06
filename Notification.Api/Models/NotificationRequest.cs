namespace Notification.Api.Models
{
    public class NotificationRequest
    {
        public string ToEmail { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string PhoneNumber { get; set; }
        public string Message { get; set; }
    }
}
