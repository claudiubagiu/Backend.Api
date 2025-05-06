using Posts.Api.Models.Domain;

namespace Posts.Api.Repositories.Interface
{
    public interface ITagsRepository
    {
        Task<Tag> CreateAsync(Tag tag);
        Task<List<Tag>> GetAllAsync();
        Task<Tag?> GetByIdAsync(Guid id);
        Task<Tag?> GetByNameAsync(string name);
        Task<Tag?> DeleteAsync(Guid id);
        Task<Tag?> UpdateAsync(Tag post);
    }
}
