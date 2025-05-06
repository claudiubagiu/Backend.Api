using Images.Api.Models.Domain;

namespace Images.Api.Repositories.Interface
{
    public interface IImageRepository
    {
        Task<Image> Upload(Image image);
        Task<List<Image>> GetAllAsync();
        Task<Image> GetByPostIdAsync(Guid postId);
        Task<Image> GetByCommentIdAsync(Guid commentId);
        Task<Image?> DeleteByPostIdAsync(Guid postId);
        Task<Image?> DeleteByCommentIdAsync(Guid commentId);
    }
}
