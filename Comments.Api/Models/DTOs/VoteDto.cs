using System.ComponentModel;
using System.Text.Json.Serialization;
using Comments.Api.Models.Domain;

namespace Comments.Api.Models.DTOs
{
    public enum Type
    {
        [Description("Like")]
        Like,

        [Description("Dislike")]
        Dislike
    }
    public class VoteDto
    {
        public required Guid Id { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public required Type Type { get; set; }
        public string UserName { get; set; }
    }
}
