using Votes.Api.Models;
using Votes.Api.Models.DTOs;

namespace Votes.Api.Repositories.Interface
{
    public interface IVotesRepository
    {
        Task<Vote> CreateAsync(Vote vote);
        Task<bool> DeleteAsync(Guid id);
        Task<bool> UpdateAsync(Vote vote);
        Task<Vote> GetById(Guid id);
        Task<List<Vote>> GetAllByPostId(Guid postId);
        Task<List<Vote>> GetAllByCommentId(Guid commentId);
        Task<bool> DeleteAllByPostId(Guid postId);
        Task<bool> DeleteAllByCommentId(Guid commentId);
        Task<Vote> GetByUserIdAndTargetIdAsync(string userId, Guid? postId, Guid? commentId);
    }
}
