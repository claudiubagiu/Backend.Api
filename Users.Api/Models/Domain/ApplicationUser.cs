using Microsoft.AspNetCore.Identity;

namespace Users.Api.Models.Domain
{
    public class ApplicationUser : IdentityUser
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public int Score { get; set; } = 0;
        public Ban? Ban { get; set; }
    }
}
