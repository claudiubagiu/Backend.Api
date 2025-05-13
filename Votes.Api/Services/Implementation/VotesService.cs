using AutoMapper;
using Votes.Api.Models;
using Votes.Api.Models.Domain;
using Votes.Api.Models.DTOs;
using Votes.Api.Repositories.Interface;
using Votes.Api.Services.Interface;

namespace Votes.Api.Services.Implementation
{
    public class VotesService : IVotesService
    {
        private readonly IVotesRepository votesRepository;
        private readonly IMapper mapper;
        private readonly IHttpClientFactory httpClientFactory;

        public VotesService(IVotesRepository votesRepository, IMapper mapper, IHttpClientFactory httpClientFactory)
        {
            this.votesRepository = votesRepository;
            this.mapper = mapper;
            this.httpClientFactory = httpClientFactory;
        }

        public async Task<VoteDto> CreateAsync(VoteRequest voteRequest)
        {
            var vote = mapper.Map<Vote>(voteRequest);
            string targetId;

            var client = httpClientFactory.CreateClient();
            if (vote.CommentId == null)
            {
                var responsePost = await client.GetAsync($"http://posts.api:5002/api/Posts/{vote.PostId}");
                var content = await responsePost.Content.ReadFromJsonAsync<Post>();
                targetId = content.UserId;
            }
            else
            {
                var responseComment = await client.GetAsync($"http://comments.api:5003/api/Comments/{vote.CommentId}");
                var content = await responseComment.Content.ReadFromJsonAsync<Comment>();
                targetId = content.UserId;
            }


            int score = voteRequest.Type == Votes.Api.Models.Type.Like ? 1 : -1;
            var response = await client.PutAsJsonAsync("http://users.api:5005/api/Users/Score", new ModifyScore
            {
                UserId = targetId,
                Score = score
            });

            vote = await votesRepository.CreateAsync(vote);
            if (vote == null)
                return null;
            return mapper.Map<VoteDto>(vote);
        }

        public async Task<bool> DeleteAsync(Guid id, VoteRequest voteRequest)
        {
            var client = httpClientFactory.CreateClient();
            int score = voteRequest.Type == Votes.Api.Models.Type.Like ? -1 : 1;
            string targetId;
            if (voteRequest.CommentId == null)
            {
                var responsePost = await client.GetAsync($"http://posts.api:5002/api/Posts/{voteRequest.PostId}");
                var content = await responsePost.Content.ReadFromJsonAsync<Post>();
                targetId = content.UserId;
            }
            else
            {
                var responseComment = await client.GetAsync($"http://comments.api:5003/api/Comments/{voteRequest.CommentId}");
                var content = await responseComment.Content.ReadFromJsonAsync<Comment>();
                targetId = content.UserId;
            }

            var response = await client.PutAsJsonAsync("http://users.api:5005/api/Users/Score", new ModifyScore
            {
                UserId = targetId,
                Score = score
            });
            var result = await votesRepository.DeleteAsync(id);
            return result;
        }

        public async Task<VoteDto> GetByIdAsync(Guid id)
        {
            var vote = await votesRepository.GetById(id);
            if (vote == null)
                return null;
            return mapper.Map<VoteDto>(vote);
        }

        public async Task<bool> UpdateAsync(Guid id, VoteRequest voteRequest)
        {
            var existingVote = await votesRepository.GetById(id);
            if (existingVote == null)
                return false;

            var client = httpClientFactory.CreateClient();
            int score = voteRequest.Type == Votes.Api.Models.Type.Like ? 2 : -2;
            string targetId;
            if (voteRequest.CommentId == null)
            {
                var responsePost = await client.GetAsync($"http://posts.api:5002/api/Posts/{voteRequest.PostId}");
                var content = await responsePost.Content.ReadFromJsonAsync<Post>();
                targetId = content.UserId;
            }
            else
            {
                var responseComment = await client.GetAsync($"http://comments.api:5003/api/Comments/{voteRequest.CommentId}");
                var content = await responseComment.Content.ReadFromJsonAsync<Comment>();
                targetId = content.UserId;
            }

            var response = await client.PutAsJsonAsync("http://users.api:5005/api/Users/Score", new ModifyScore
            {
                UserId = targetId,
                Score = score
            });

            var vote = mapper.Map<Vote>(voteRequest);
            vote.Id = id;
            var result = await votesRepository.UpdateAsync(vote);
            return result;
        }

        public async Task<NrVotes> GetAllByPostIdAsync(Guid postId)
        {
            var votes = await votesRepository.GetAllByPostId(postId);
            if (votes == null)
                return null;
            int likes = votes.Count(votes => votes.Type == Votes.Api.Models.Type.Like);
            int dislikes = votes.Count(votes => votes.Type == Votes.Api.Models.Type.Dislike);
            var nrVotes = new NrVotes
            {
                Likes = likes,
                Dislikes = dislikes
            };
            return nrVotes;
        }

        public async Task<NrVotes> GetAllByCommentIdAsync(Guid commentId)
        {
            var votes = await votesRepository.GetAllByCommentId(commentId);
            if (votes == null)
                return null;
            int likes = votes.Count(votes => votes.Type == Votes.Api.Models.Type.Like);
            int dislikes = votes.Count(votes => votes.Type == Votes.Api.Models.Type.Dislike);
            var nrVotes = new NrVotes
            {
                Likes = likes,
                Dislikes = dislikes
            };
            return nrVotes;
        }

        public async Task<bool> DeleteAllByPostIdAsync(Guid postId)
        {
            var votes = await votesRepository.GetAllByPostId(postId);
            if (votes == null || votes.Count == 0)
                return false;
            foreach (var vote in votes)
            {
                var client = httpClientFactory.CreateClient();
                int score = vote.Type == Votes.Api.Models.Type.Like ? -1 : 1;
                string targetId;
                if (vote.CommentId == null)
                {
                    var responsePost = await client.GetAsync($"http://posts.api:5002/api/Posts/{vote.PostId}");
                    var content = await responsePost.Content.ReadFromJsonAsync<Post>();
                    targetId = content.UserId;
                }
                else
                {
                    var responseComment = await client.GetAsync($"http://comments.api:5003/api/Comments/{vote.CommentId}");
                    var content = await responseComment.Content.ReadFromJsonAsync<Comment>();
                    targetId = content.UserId;
                }
                var response = await client.PutAsJsonAsync("http://users.api:5005/api/Users/Score", new ModifyScore
                {
                    UserId = targetId,
                    Score = score
                });
            }
            var result = await votesRepository.DeleteAllByPostId(postId);
            return result;
        }

        public async Task<bool> DeleteAllByCommentIdAsync(Guid commentId)
        {
            var votes = await votesRepository.GetAllByCommentId(commentId);
            if (votes == null || votes.Count == 0)
                return false;
            foreach (var vote in votes)
            {
                var client = httpClientFactory.CreateClient();
                int score = vote.Type == Votes.Api.Models.Type.Like ? -1 : 1;
                string targetId;
                if (vote.CommentId == null)
                {
                    var responsePost = await client.GetAsync($"http://posts.api:5002/api/Posts/{vote.PostId}");
                    var content = await responsePost.Content.ReadFromJsonAsync<Post>();
                    targetId = content.UserId;
                }
                else
                {
                    var responseComment = await client.GetAsync($"http://comments.api:5003/api/Comments/{vote.CommentId}");
                    var content = await responseComment.Content.ReadFromJsonAsync<Comment>();
                    targetId = content.UserId;
                }
                var response = await client.PutAsJsonAsync("http://users.api:5005/api/Users/Score", new ModifyScore
                {
                    UserId = targetId,
                    Score = score
                });
            }
            var result = await votesRepository.DeleteAllByCommentId(commentId);
            return result;
        }

        public async Task<VoteDto> GetByUserIdAndTargetIdAsync(string userId, Guid? postId, Guid? commentId)
        {
            var vote = await votesRepository.GetByUserIdAndTargetIdAsync(userId, postId, commentId);
            if (vote == null)
                return null;
            return mapper.Map<VoteDto>(vote);
        }
        public async Task<bool> CheckUserVoteAsync(string userId, Guid? postId, Guid? commentId)
        {
            if (postId != null)
            {
                var post = await votesRepository.GetPostById((Guid)postId);
                if (post.UserId == userId)
                    return false;
            }
            if (commentId != null)
            {
                var comment = await votesRepository.GetCommentById((Guid)commentId);
                if (comment.UserId == userId)
                    return false;
            }
            return true;
        }
    }
}
