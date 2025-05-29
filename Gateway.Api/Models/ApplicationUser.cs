using Microsoft.AspNetCore.Identity;

namespace Gateway.Api.Models
{
    public class ApplicationUser : IdentityUser
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public float Score { get; set; } = 0;
        public Ban? Ban { get; set; }
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public ICollection<Vote> Votes { get; set; } = new List<Vote>();
        public ICollection<Post> Posts { get; set; } = new List<Post>();
    }
}
