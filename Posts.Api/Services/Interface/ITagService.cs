using Posts.Api.Models.Domain;
using Posts.Api.Models.DTOs;

namespace Posts.Api.Services.Interface
{
    public interface ITagService
    {
        Task<TagDto?> CreateAsync(AddTagRequestDto addTagRequestDto);
        Task<List<TagDto>?> GetAllAsync();
        Task<Boolean> DeleteAsync(Guid id);
        Task<TagDto?> GetByIdAsync(Guid id);

    }
}
