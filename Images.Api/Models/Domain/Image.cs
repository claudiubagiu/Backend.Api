using System.ComponentModel.DataAnnotations.Schema;

namespace Images.Api.Models.Domain
{
    public class Image
    {
        public required Guid Id { get; set; }
        [NotMapped]
        public required IFormFile File { get; set; }
        public required string FileName { get; set; }
        public string? FileDescription { get; set; }
        public required string FileExtension { get; set; }
        public long FileSizeInBytes { get; set; }
        public required string FilePath { get; set; }
        public Guid? CommentId { get; set; }
        public Guid? PostId { get; set; }

    }
}
