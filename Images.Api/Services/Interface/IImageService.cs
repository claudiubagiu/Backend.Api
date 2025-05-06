using Images.Api.Models.Domain;
using Images.Api.Models.DTOs;

namespace Images.Api.Services.Interface
{
    public interface IImageService
    {
        Task<Image> Upload(UploadImageRequestDto image);
        Task<List<ImageDto>?> GetAllAsync();
        Task<ImageDto?> GetByPostIdAsync(Guid postId);
        Task<ImageDto?> GetByCommentIdAsync(Guid commentId);
        Task<Boolean> DeleteByPostIdAsync(Guid postId);
        Task<Boolean> DeleteByCommentIdAsync(Guid commentId);
    }
}
