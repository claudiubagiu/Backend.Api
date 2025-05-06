using System.ComponentModel;
using System.Text.Json.Serialization;
using Votes.Api.Models.Domain;

namespace Votes.Api.Models
{
    public class VoteDto
    {
        public required Guid Id { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]

        public required Type Type { get; set; }
        public required string UserId { get; set; }
        public Guid? CommentId { get; set; }
        public Comment? Comment { get; set; }
        public Guid? PostId { get; set; }
        public Post? Post { get; set; }
    }
}
