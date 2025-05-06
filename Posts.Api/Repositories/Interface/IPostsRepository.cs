using Posts.Api.Models.Domain;

namespace Posts.Api.Repositories.Interface
{
    public interface IPostsRepository
    {
        Task<Post> CreateAsync(Post post);
        Task<List<Post>> GetAllAsync();
        Task<Post?> GetByIdAsync(Guid id);
        Task<Post?> DeleteAsync(Guid id);
        Task<Post?> UpdateAsync(Post post);
    }
}
