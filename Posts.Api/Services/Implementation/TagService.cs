using AutoMapper;
using Posts.Api.Models.Domain;
using Posts.Api.Models.DTOs;
using Posts.Api.Repositories.Interface;
using Posts.Api.Services.Interface;

namespace Posts.Api.Services.Implementation
{
    public class TagService : ITagService
    {
        private readonly ITagsRepository tagsRepository;
        private readonly IMapper mapper;

        public TagService(ITagsRepository tagsRepository, IMapper mapper)
        {
            this.tagsRepository = tagsRepository;
            this.mapper = mapper;
        }

        public async Task<TagDto?> CreateAsync(AddTagRequestDto addTagRequestDto)
        {
            Tag tag = mapper.Map<Tag>(addTagRequestDto);
            tag = await tagsRepository.CreateAsync(tag);
            return mapper.Map<TagDto>(tag);
        }

        public async Task<Boolean> DeleteAsync(Guid id)
        {
            var tag = await tagsRepository.GetByIdAsync(id);
            if (tag == null)
                return false;

            await tagsRepository.DeleteAsync(id);
            return true;
        }

        public async Task<List<TagDto>?> GetAllAsync()
        {
            var tags = await tagsRepository.GetAllAsync();
            if (tags == null)
                return null;
            return mapper.Map<List<TagDto>>(tags);
        }

        public async Task<TagDto?> GetByIdAsync(Guid id)
        {
            var post = await tagsRepository.GetByIdAsync(id);
            if (post == null)
                return null;
            return mapper.Map<TagDto>(post);
        }
    }
}
