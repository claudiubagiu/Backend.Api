using Posts.Api.Models.Domain;
using Posts.Api.Models.DTOs;
using Posts.Api.Repositories.Interface;
using Posts.Api.Services.Interface;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.IO.Pipelines;
using System.Text.Json;

namespace Posts.Api.Services.Implementation
{
    public class PostService : IPostService
    {
        private readonly IMapper mapper;
        private readonly IPostsRepository postRepository;
        private readonly ITagsRepository tagsRepository;
        private readonly IHttpClientFactory httpClientFactory;

        public PostService(IMapper mapper, IPostsRepository postRepository, ITagsRepository tagsRepository, IHttpClientFactory httpClientFactory)
        {
            this.mapper = mapper;
            this.postRepository = postRepository;
            this.tagsRepository = tagsRepository;
            this.httpClientFactory = httpClientFactory;
        }

        public async Task<List<PostDto>?> GetAllAsync()
        {
            var client = httpClientFactory.CreateClient();
            var posts = await postRepository.GetAllAsync();
            var postsDto = mapper.Map<List<PostDto>>(posts);
            for (int i = 0; i < postsDto.Count; i++)
            {
                var postDto = postsDto[i];
                var post = posts[i];

                var response = await client.GetAsync($"http://images.api:5004/api/Images/{postDto.Id}");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadFromJsonAsync<ImageDto>();
                    postDto.FeaturedImageUrl = content.FilePath;
                }
                response = await client.GetAsync($"http://comments.api:5003/api/Comments/Post/{postDto.Id}");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadFromJsonAsync<List<CommentDto>>();
                    postDto.Comments = content;
                }
                response = await client.GetAsync($"http://votes.api:5006/api/Votes/Post/{postDto.Id}");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadFromJsonAsync<NrVotes>();
                    postDto.NrVotes = content;
                }
                response = await client.GetAsync($"http://users.api:5005/api/Users/{post.UserId}");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadFromJsonAsync<ApplicationUser>();
                    postDto.Username = content.UserName;
                }
            }
            if (postsDto != null)
                return postsDto;
            return null;
        }

        public async Task<PostDto?> CreateAsync(AddPostRequestDto addPostRequestDto, DateTime dateTime, string userId)
        {
            Post post = mapper.Map<Post>(addPostRequestDto);
            post.CreatedAt = dateTime;
            post.UserId = userId;

            var tagsName = addPostRequestDto.TagsName;

            foreach (var tagName in tagsName)
            {
                var tag = await tagsRepository.GetByNameAsync(tagName);
                if (tag == null)
                {
                    tag = new Tag { Name = tagName };
                    await tagsRepository.CreateAsync(tag);
                }
                post.Tags.Add(tag);
            }

            post = await postRepository.CreateAsync(post);

            if (post != null)
                return mapper.Map<PostDto>(post);
            return null;
        }

        public async Task<Boolean> DeleteAsync(Guid id, string userId, List<String> userRoleClaims)
        {
            var post = await postRepository.GetByIdAsync(id);

            if (post == null)
                return false;

            var client = httpClientFactory.CreateClient();
            var response = await client.DeleteAsync($"http://images.api:5004/api/Images/Post/{id}");
            response = await client.DeleteAsync($"http://votes.api:5006/api/Votes/Post/{id}");

            response = await client.GetAsync($"http://comments.api:5003/api/Comments/Post/{id}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadFromJsonAsync<List<CommentDto>>();
                foreach (var comment in content)
                {
                    response = await client.DeleteAsync($"http://votes.api:5006/api/Votes/Comment/{comment.Id}");
                }
            }

            if (userRoleClaims.Contains("Admin") || post.UserId == userId)
            {
                await postRepository.DeleteAsync(id);
                return true;
            }
            return false;
        }

        public async Task<PostDto?> GetByIdAsync(Guid id)
        {
            var post = await postRepository.GetByIdAsync(id);
            if (post != null)
                return mapper.Map<PostDto>(post);
            return null;
        }

        public async Task<PostDto?> UpdateAsync(Guid id, UpdatePostRequestDto updatePostRequestDto, string userId, List<String> userRoleClaims)
        {
            var post = await postRepository.GetByIdAsync(id);
            if (post == null)
                return null;

            if (userRoleClaims.Contains("Admin") || post.UserId == userId)
            {
                post = mapper.Map(updatePostRequestDto, post);
                post = await postRepository.UpdateAsync(post);
                return mapper.Map<PostDto>(post);
            }
            return null;
        }
    }
}
