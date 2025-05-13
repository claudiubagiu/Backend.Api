using System.ComponentModel;
using Comments.Api.Models.Domain;

namespace Comments.Api.Models
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
        public ApplicationUser User { get; set; }
        public Guid? CommentId { get; set; }
        public Comment? Comment { get; set; }
        public Guid? PostId { get; set; }
    }
}
