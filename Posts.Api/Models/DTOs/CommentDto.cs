using Posts.Api.Models.Domain;
using System.Text.Json.Serialization;

namespace Posts.Api.Models.DTOs
{
    public class CommentDto
    {
        public required Guid Id { get; set; }
        public required string Description { get; set; }
        public required DateTime CreatedAt { get; set; }
        public required string UserId { get; set; }
        public Guid PostId { get; set; }
        public Guid? CommentId { get; set; }
        public required string FeaturedImageUrl { get; set; }
        public ICollection<CommentDto> ChildrenComments { get; set; } = new List<CommentDto>();
    }
}
