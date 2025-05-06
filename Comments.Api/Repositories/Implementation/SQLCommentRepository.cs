using Comments.Api.Data;
using Comments.Api.Models.Domain;
using Comments.Api.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace Comments.Api.Repositories.Implementation
{
    public class SQLCommentRepository : ICommentRepository
    {
        private readonly BackendDbContext dbContext;

        public SQLCommentRepository(BackendDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<Comment> CreateAsync(Comment comment)
        {
            await dbContext.Comment.AddAsync(comment);
            await dbContext.SaveChangesAsync();
            return comment;
        }

        public async Task<Comment?> DeleteAsync(Guid id)
        {
            var comment = await dbContext.Comment.Include("ChildrenComments").FirstOrDefaultAsync(c => c.Id == id);
            if (comment == null)
                return null;

            dbContext.Comment.Remove(comment);
            await dbContext.SaveChangesAsync();
            return comment;
        }

        public async Task<List<Comment>> GetAllAsync()
        {
            return await dbContext.Comment.Include("ChildrenComments").Where(c => c.CommentId == null).ToListAsync();
        }

        public async Task<Comment?> GetByIdAsync(Guid id)
        {
            return await dbContext.Comment.Include("ChildrenComments").FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Comment?> UpdateAsync(Comment comment)
        {
            var existingComment = dbContext.Comment.FirstOrDefault(c => c.Id == comment.Id);

            if (existingComment == null)
                return null;

            dbContext.Entry(existingComment).CurrentValues.SetValues(comment);

            await dbContext.SaveChangesAsync();
            return existingComment;
        }

        public async Task<List<Comment>> GetAllByPostIdAsync(Guid postId)
        {
            return await dbContext.Comment.Include("ChildrenComments").Where(c => c.PostId == postId && c.CommentId == null).ToListAsync();
        }
    }
}
