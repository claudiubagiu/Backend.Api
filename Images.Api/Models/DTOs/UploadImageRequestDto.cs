namespace Images.Api.Models.DTOs
{
    public class UploadImageRequestDto
    {
        public required IFormFile File { get; set; }
        public required string FileName { get; set; }
        public string? FileDescription { get; set; }
        public string? CommentId { get; set; }
        public string? PostId { get; set; }
    }
}
