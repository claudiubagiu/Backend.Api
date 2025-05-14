using Posts.Api.Models.DTOs;

namespace Posts.Api.Services.Interface
{
    public interface IPostService
    {
        Task<PostDto?> CreateAsync(AddPostRequestDto addPostRequestDto, DateTime dateTime, string userId);
        Task<PostDto?> GetByIdAsync(Guid id);
        Task<List<PostDto>?> GetAllAsync();
        Task<PostDto?> UpdateAsync(Guid id, UpdatePostRequestDto updatePostRequestDto, string userId, List<String> userRoleClaims);
        Task<Boolean> DeleteAsync(Guid id, string userId, List<String> userRoleClaims);
        Task<Boolean> ChangePostStatus(Guid id);
        Task<List<PostDto>?> FilterQuestionsAsync(PostsFilterDto filters, string? currentUserId);
    }
}
