using Comments.Api.Models.Domain;
using Comments.Api.Models.DTOs;
using Comments.Api.Repositories.Interface;
using Comments.Api.Services.Interface;
using AutoMapper;
using AutoMapper.Configuration.Annotations;

namespace Comments.Api.Services.Implementation
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository commentRepository;
        private readonly IMapper mapper;
        private readonly IHttpClientFactory httpClientFactory;

        public CommentService(ICommentRepository commentRepository, IMapper mapper, IHttpClientFactory httpClientFactory)
        {
            this.commentRepository = commentRepository;
            this.mapper = mapper;
            this.httpClientFactory = httpClientFactory;
        }
        public async Task<CommentDto?> CreateAsync(AddCommentRequestDto addCommentRequestDto, DateTime dateTime, string userId)
        {
            Comment comment = mapper.Map<Comment>(addCommentRequestDto);
            comment.CreatedAt = dateTime;
            comment.UserId = userId;
            comment = await commentRepository.CreateAsync(comment);

            if (comment != null)
                return mapper.Map<CommentDto>(comment);
            return null;
        }

        public async Task<bool> DeleteAsync(Guid id, string userId, List<String> userRoleClaims)
        {
            var comment = await commentRepository.GetByIdAsync(id);

            if (comment == null)
                return false;

            //var client = httpClientFactory.CreateClient();
            //var response = await client.DeleteAsync($"http://images.api:5004/api/Images/Comment/{id}");

            var client = httpClientFactory.CreateClient();
            var response = await client.DeleteAsync($"http://votes.api:5006/api/Votes/Comment/{id}");

            if (userRoleClaims.Contains("Admin") || comment.UserId == userId)
            {
                await DeleteChildrenAsync(comment.ChildrenComments.ToList());
                await commentRepository.DeleteAsync(id);
                return true;
            }
            return false;
        }

        private async Task DeleteChildrenAsync(List<Comment> children)
        {
            if (!children.Any()) return;

            foreach (var child in children)
            {
                if (child.ChildrenComments.Any())
                    await DeleteChildrenAsync(child.ChildrenComments.ToList());

                var client = httpClientFactory.CreateClient();
                var response = await client.DeleteAsync($"http://votes.api:5006/api/Votes/Comment/{child.Id}");
                await commentRepository.DeleteAsync(child.Id);
            }
        }
        public async Task<List<CommentDto>?> GetAllAsync()
        {
            var comments = await commentRepository.GetAllAsync();
            var commentsDto = mapper.Map<List<CommentDto>>(comments);
            await GetImagesAsync(commentsDto);

            if (commentsDto != null)
                return commentsDto;
            return null;
        }

        private async Task GetImagesAsync(List<CommentDto> children)
        {
            if (!children.Any()) return;

            foreach (var child in children)
            {
                if (child.ChildrenComments.Any())
                    await GetImagesAsync(child.ChildrenComments.ToList());

                var client = httpClientFactory.CreateClient();
                var response = await client.GetAsync($"http://images.api:5004/api/Images/Comment/{child.Id}");
                if (response.IsSuccessStatusCode)
                {
                    var imageUrl = await response.Content.ReadFromJsonAsync<ImageDto>();
                    child.FeaturedImageUrl = imageUrl.FilePath;
                }
            }
        }

        public async Task<CommentDto?> GetByIdAsync(Guid id)
        {
            var comment = await commentRepository.GetByIdAsync(id);
            if (comment != null)
                return mapper.Map<CommentDto>(comment);
            return null;
        }

        public async Task<CommentDto?> UpdateAsync(Guid id, UpdateCommentRequestDto updateCommentRequestDto, string userId, List<String> userRoleClaims)
        {
            var comment = await commentRepository.GetByIdAsync(id);
            if (comment == null)
                return null;

            if (userRoleClaims.Contains("Admin") || comment.UserId == userId)
            {
                comment = mapper.Map(updateCommentRequestDto, comment);
                comment = await commentRepository.UpdateAsync(comment);
                return mapper.Map<CommentDto>(comment);
            }
            return null;
        }

        public async Task<List<CommentDto>?> GetAllByPostIdAsync(Guid postId)
        {
            var comments = await commentRepository.GetAllByPostIdAsync(postId);
            var commentsDto = mapper.Map<List<CommentDto>>(comments);
            await GetImagesAsync(commentsDto);
            if (commentsDto != null)
                return commentsDto;
            return null;
        }
    }
}
