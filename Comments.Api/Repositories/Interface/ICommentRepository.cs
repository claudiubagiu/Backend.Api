using Comments.Api.Models.Domain;

namespace Comments.Api.Repositories.Interface
{
    public interface ICommentRepository
    {
        Task<Comment> CreateAsync(Comment comment);
        Task<List<Comment>> GetAllAsync();
        Task<List<Comment>> GetAllByPostIdAsync(Guid postId);
        Task<Comment?> GetByIdAsync(Guid id);
        Task<Comment?> DeleteAsync(Guid id);
        Task<Comment?> UpdateAsync(Comment comment);
    }
}
