using Microsoft.EntityFrameworkCore;
using Posts.Api.Data;
using Posts.Api.Models.Domain;
using Posts.Api.Repositories.Interface;

namespace Posts.Api.Repositories.Implementation
{
    public class TagsRepository : ITagsRepository
    {
        private readonly BackendDbContext dbContext;

        public TagsRepository(BackendDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Tag> CreateAsync(Tag tag)
        {
            await dbContext.Tag.AddAsync(tag);
            await dbContext.SaveChangesAsync();
            return tag;

        }

        public async Task<Tag?> DeleteAsync(Guid id)
        {
            var tag = dbContext.Tag.FirstOrDefault(x => x.Id == id);
            if (tag == null)
                return null;
            dbContext.Tag.Remove(tag);
            await dbContext.SaveChangesAsync();
            return tag;
        }

        public async Task<List<Tag>> GetAllAsync()
        {
            return await dbContext.Tag.ToListAsync();
        }

        public async Task<Tag?> GetByIdAsync(Guid id)
        {
            return await dbContext.Tag.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Tag?> GetByNameAsync(string name)
        {
            return await dbContext.Tag.FirstOrDefaultAsync(x => x.Name == name);
        }

        public async Task<Tag?> UpdateAsync(Tag post)
        {
            var existingTag = await dbContext.Tag.FirstOrDefaultAsync(x => x.Id == post.Id);

            if (existingTag == null)
                return null;

            dbContext.Entry(existingTag).CurrentValues.SetValues(post);
            await dbContext.SaveChangesAsync();
            return existingTag;
        }
    }
}