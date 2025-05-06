namespace Votes.Api.Models.DTOs
{
    public class VoteRequest
    {
        public required string UserId { get; set; }
        public Guid? CommentId { get; set; }
        public Guid? PostId { get; set; }
        public required Type Type { get; set; }
    }
}
