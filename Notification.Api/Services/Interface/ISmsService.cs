namespace Notification.Api.Services.Interface
{
    public interface ISmsService
    {
        Task SendSmsAsync(string toPhoneNumber, string message);
    }
}
