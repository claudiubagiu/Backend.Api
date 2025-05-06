using Microsoft.EntityFrameworkCore;
using Votes.Api.Data;
using Votes.Api.Models;
using Votes.Api.Models.DTOs;
using Votes.Api.Repositories.Interface;

namespace Votes.Api.Repositories.Implementation
{
    public class VotesRepository : IVotesRepository
    {
        private readonly BackendDbContext dbContext;

        public VotesRepository(BackendDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<Vote> CreateAsync(Vote vote)
        {
            await dbContext.Vote.AddAsync(vote);
            await dbContext.SaveChangesAsync();
            return vote;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var vote = await dbContext.Vote.FirstOrDefaultAsync(v => v.Id == id);
            if (vote == null)
                return false;
            dbContext.Vote.Remove(vote);
            await dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<Vote> GetById(Guid id)
        {
            var vote = await dbContext.Vote.FirstOrDefaultAsync(v => v.Id == id);
            if (vote == null)
                return null;
            return vote;
        }

        public async Task<bool> UpdateAsync(Vote vote)
        {
            var existingVote = await dbContext.Vote.FirstOrDefaultAsync(v => v.Id == vote.Id);
            if (existingVote == null)
                return false;
            dbContext.Entry(existingVote).CurrentValues.SetValues(vote);
            await dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<List<Vote>> GetAllByPostId(Guid postId)
        {
            return await dbContext.Vote.Where(v => v.PostId == postId).ToListAsync();
        }

        public async Task<List<Vote>> GetAllByCommentId(Guid commentId)
        {
            return await dbContext.Vote.Where(v => v.CommentId == commentId).ToListAsync();
        }

        public async Task<bool> DeleteAllByPostId(Guid postId)
        {
            var votes = await dbContext.Vote.Where(v => v.PostId == postId).ToListAsync();
            if (votes == null || votes.Count == 0)
                return false;
            dbContext.Vote.RemoveRange(votes);
            await dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAllByCommentId(Guid commentId)
        {
            var votes = await dbContext.Vote.Where(v => v.CommentId == commentId).ToListAsync();
            if (votes == null || votes.Count == 0)
                return false;
            dbContext.Vote.RemoveRange(votes);
            await dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<Vote> GetByUserIdAndTargetIdAsync(string userId, Guid? postId, Guid? commentId)
        {
            var vote = await dbContext.Vote.FirstOrDefaultAsync(v => v.UserId == userId && v.PostId == postId && v.CommentId == commentId);
            if (vote == null)
                return null;
            return vote;
        }
    }
}
