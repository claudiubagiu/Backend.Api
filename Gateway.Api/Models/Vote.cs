using System.ComponentModel;

namespace Gateway.Api.Models
{
    public enum Type
    {
        [Description("Like")]
        Like,

        [Description("Dislike")]
        Dislike
    }
    public class Vote
    {
        public required Guid Id { get; set; }
        public required Type Type { get; set; }
        public required string UserId { get; set; }
        public ApplicationUser User { get; set; } = null!;
        public Guid? CommentId { get; set; }
        public Comment? Comment { get; set; }
        public Guid? PostId { get; set; }
        public Post? Post { get; set; }
    }
}
