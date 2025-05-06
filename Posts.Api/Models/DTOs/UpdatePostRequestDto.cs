namespace Posts.Api.Models.DTOs
{
    public class UpdatePostRequestDto
    {
        public required string Title { get; set; }
        public required string Description { get; set; }
        public required Status Status { get; set; }
    }
}
