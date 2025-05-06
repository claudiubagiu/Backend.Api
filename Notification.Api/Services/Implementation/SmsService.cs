using Twilio.Types;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Notification.Api.Services.Interface;

namespace Notification.Api.Services.Implementation
{
    public class SmsService : ISmsService
    {
        private readonly string _accountSid;
        private readonly string _authToken;
        private readonly string _fromPhoneNumber;

        public SmsService(IConfiguration configuration)
        {
            _accountSid = configuration["Twilio:AccountSid"];
            _authToken = configuration["Twilio:AuthToken"];
            _fromPhoneNumber = configuration["Twilio:FromPhoneNumber"];

            TwilioClient.Init(_accountSid, _authToken);
        }

        public async Task SendSmsAsync(string toPhoneNumber, string message)
        {
            var messageResource = await MessageResource.CreateAsync(
                to: new PhoneNumber(toPhoneNumber),
                from: new PhoneNumber(_fromPhoneNumber),
                body: message
            );
        }
    }
}
