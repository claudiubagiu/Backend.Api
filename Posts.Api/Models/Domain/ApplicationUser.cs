using Microsoft.AspNetCore.Identity;

namespace Posts.Api.Models.Domain
{
    public class ApplicationUser : IdentityUser
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public float Score { get; set; } = 0;
    }
}
