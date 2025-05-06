using Comments.Api.Models.Domain;

namespace Comments.Api.Models.DTOs
{
    public class AddCommentRequestDto
    {
        public required string Description { get; set; }
        public Guid? CommentId { get; set; }
        public Guid? PostId { get; set; }

    }
}
