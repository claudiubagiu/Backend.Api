using System.ComponentModel;
using System.Text.Json.Serialization;
using Posts.Api.Models.Domain;

namespace Posts.Api.Models.DTOs
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
