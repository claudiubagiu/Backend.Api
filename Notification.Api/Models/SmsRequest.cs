﻿namespace Notification.Api.Models
{
    public class SmsRequest
    {
        public string PhoneNumber { get; set; }
        public string Message { get; set; }
    }
}
