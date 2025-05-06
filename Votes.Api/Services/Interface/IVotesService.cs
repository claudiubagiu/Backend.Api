using Votes.Api.Models;
using Votes.Api.Models.DTOs;

namespace Votes.Api.Services.Interface
{
    public interface IVotesService
    {
        Task<VoteDto> CreateAsync(VoteRequest voteRequest);
        Task<bool> DeleteAsync(Guid id, VoteRequest voteRequest);
        Task<bool> UpdateAsync(Guid id, VoteRequest voteRequest);
        Task<VoteDto> GetByIdAsync(Guid id);
        Task<NrVotes> GetAllByPostIdAsync(Guid postId);
        Task<NrVotes> GetAllByCommentIdAsync(Guid commentId);
        Task<bool> DeleteAllByPostIdAsync(Guid postId);
        Task<bool> DeleteAllByCommentIdAsync(Guid commentId);
        Task<VoteDto> GetByUserIdAndTargetIdAsync(string userId, Guid? postId, Guid? commentId);
    }
}
