﻿using Posts.Api.Models.Domain;
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
            var posts = await postRepository.GetAllAsync();
            var postsDto = mapper.Map<List<PostDto>>(posts);
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

        public async Task<Boolean> ChangePostStatus(Guid id)
        {
            var post = await postRepository.GetByIdAsync(id);
            if (post == null)
                return false;
            post.Status = Models.Domain.Status.Resolved;
            await postRepository.UpdateAsync(post);
            return true;
        }

        public async Task<List<PostDto>> FilterQuestionsAsync(PostsFilterDto filters, string? currentUserId)
        {
            var posts = await postRepository.GetAllAsync();
            var query = posts.AsQueryable();

            if (!string.IsNullOrEmpty(filters.Tag))
            {
                query = query.Where(q => q.Tags.Any(t => t.Name == filters.Tag));
            }

            if (!string.IsNullOrEmpty(filters.SearchText))
            {
                query = query.Where(q => q.Title.Contains(filters.SearchText));
            }

            if (!string.IsNullOrEmpty(filters.UserId))
            {
                query = query.Where(q => q.UserId == filters.UserId);
            }

            if (!string.IsNullOrEmpty(filters.UserName))
            {
                query = query.Where(q => q.User.UserName == filters.UserName);
            }

            if (filters.OnlyOwnQuestions && !string.IsNullOrEmpty(currentUserId))
            {
                query = query.Where(q => q.UserId == currentUserId);
            }

            var postsDto = mapper.Map<List<PostDto>>(query.ToList());
            return postsDto;
        }
    }
}
