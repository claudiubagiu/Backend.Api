using Posts.Api.Models.Domain;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace Posts.Api.Models.DTOs
{
    public enum Status
    {
        [Description("Received")]
        Received,

        [Description("Ongoing")]
        Ongoing,

        [Description("Resolved")]
        Resolved
    }
    public class PostDto
    {
        public required Guid Id { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public required DateTime CreatedAt { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public required Status Status { get; set; }
        public required string UserId { get; set; }
        public List<TagDto> Tags { get; set; } = new List<TagDto>();
        public List<CommentDto> Comments { get; set; } = new List<CommentDto>();
        public List<VoteDto> Votes { get; set; } = new List<VoteDto>();
        public ApplicationUserDto User { get; set; }
        public NrVotes NrVotes { get; set; }
        public ImageDto? Image { get; set; }
    }
}
