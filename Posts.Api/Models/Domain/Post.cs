using System.ComponentModel;

namespace Posts.Api.Models.Domain
{
    public enum Status
    {
        [Description("Received")]
        Received,

        [Description("In Progress")]
        InProgress,

        [Description("Resolved")]
        Resolved
    }
    public class Post
    {
        public required Guid Id { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public required DateTime CreatedAt { get; set; }
        public required Status Status { get; set; }
        public required string UserId { get; set; }
        public List<Tag> Tags { get; set; } = new List<Tag>();
    }
}
