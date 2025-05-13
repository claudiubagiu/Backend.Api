using Microsoft.EntityFrameworkCore;
using Posts.Api.Data;
using Posts.Api.Models.Domain;
using Posts.Api.Repositories.Interface;

namespace Posts.Api.Repositories.Implementation
{
    public class PostsRepository : IPostsRepository
    {
        private readonly BackendDbContext dbContext;

        public PostsRepository(BackendDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<List<Post>> GetAllAsync()
        {
            return await dbContext.Post
                .OrderByDescending(p => p.CreatedAt)
                .Include(p => p.Tags)
                .Include(p => p.Image)
                .Include(p => p.Comments)
                .Include(p => p.User)
                .Include(p => p.Votes)
                    .ThenInclude(v => v.User)
                .ToListAsync();
        }

        public async Task<Post> CreateAsync(Post post)
        {
            await dbContext.Post.AddAsync(post);
            await dbContext.SaveChangesAsync();
            return post;
        }

        public async Task<Post?> DeleteAsync(Guid id)
        {
            var post = await dbContext.Post.FirstOrDefaultAsync(p => p.Id == id);

            if (post == null)
            {
                return null;
            }
            dbContext.Post.Remove(post);
            await dbContext.SaveChangesAsync();
            return post;
        }

        public async Task<Post?> GetByIdAsync(Guid id)
        {
            return await dbContext.Post
                .Include(p => p.Tags)
                .Include(p => p.Image)
                .Include(p => p.Comments)
                .Include(p => p.User)
                .Include(p => p.Votes)
                    .ThenInclude(v => v.User)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Post?> UpdateAsync(Post post)
        {
            var existingPost = await dbContext.Post.FirstOrDefaultAsync(p => p.Id == post.Id);

            if (existingPost == null)
            {
                return null;
            }

            dbContext.Entry(existingPost).CurrentValues.SetValues(post);

            await dbContext.SaveChangesAsync();
            return existingPost;
        }
    }
}
