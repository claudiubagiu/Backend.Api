using Posts.Api.Models.Domain;

namespace Posts.Api.Models.DTOs
{
    public class AddPostRequestDto
    {
        public required string Title { get; set; }
        public required string Description { get; set; }
        public List<string> TagsName { get; set; } = new List<string>();
    }
}
