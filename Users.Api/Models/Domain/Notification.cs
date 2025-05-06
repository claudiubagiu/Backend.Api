namespace Users.Api.Models.Domain
{
    public class Notification
    {
        public string ToEmail { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string PhoneNumber { get; set; }
        public string Message { get; set; }
    }
}
