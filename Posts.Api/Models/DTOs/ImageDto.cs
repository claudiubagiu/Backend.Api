using System.ComponentModel.DataAnnotations.Schema;

namespace Posts.Api.Models.DTOs
{
    public class ImageDto
    {
        public required Guid Id { get; set; }
        public required string FileName { get; set; }
        public string? FileDescription { get; set; }
        public required string FileExtension { get; set; }
        public long FileSizeInBytes { get; set; }
        public required string FilePath { get; set; }
    }
}
