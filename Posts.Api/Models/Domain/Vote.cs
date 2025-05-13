using System.ComponentModel;
using Posts.Api.Models.Domain;

namespace Posts.Api.Models
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
    }
}
