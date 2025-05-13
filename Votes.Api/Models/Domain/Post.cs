using System.ComponentModel;

namespace Votes.Api.Models.Domain
{
    public class Post
    {
        public required Guid Id { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public required DateTime CreatedAt { get; set; }
        public required string UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}
