namespace Posts.Api.Models.DTOs
{
    public class PostsFilterDto
    {
        public string? Tag { get; set; }
        public string? SearchText { get; set; }
        public string? UserId { get; set; }
        public bool OnlyOwnQuestions { get; set; } = false;
    }
}
