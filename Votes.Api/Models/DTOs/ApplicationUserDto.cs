using Microsoft.AspNetCore.Identity;

namespace Votes.Api.Models.Domain
{
    public class ApplicationUserDto
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string UserName { get; set; }
        public float Score { get; set; } = 0;
    }
}
