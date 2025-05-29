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

        private float GetScoreDelta(Models.Type type, bool isAnswer, bool isVoteRemoval = false)
        {
            float delta = 0;
            if (type == Models.Type.Like)
                delta = isAnswer ? 5f : 2.5f;
            else if (type == Models.Type.Dislike)
                delta = isAnswer ? -2.5f : -1.5f;

            return isVoteRemoval ? -delta : delta;
        }

        public async Task<VoteDto> CreateAsync(VoteRequest voteRequest)
        {
            var vote = mapper.Map<Vote>(voteRequest);
            string targetId;

            var client = httpClientFactory.CreateClient();
            bool isAnswer;

            if (vote.CommentId == null)
            {
                var responsePost = await client.GetAsync($"http://posts.api:5002/api/Posts/{vote.PostId}");
                var content = await responsePost.Content.ReadFromJsonAsync<Post>();
                targetId = content.UserId;
                isAnswer = false;
            }
            else
            {
                var responseComment = await client.GetAsync($"http://comments.api:5003/api/Comments/{vote.CommentId}");
                var content = await responseComment.Content.ReadFromJsonAsync<Comment>();
                targetId = content.UserId;
                isAnswer = true;
            }

            float score = GetScoreDelta(voteRequest.Type, isAnswer);
            await client.PutAsJsonAsync("http://users.api:5005/api/Users/Score", new ModifyScore
            {
                UserId = targetId,
                Score = score
            });

            // Penalize voter if they downvote someone else's answer
            if (voteRequest.Type == Models.Type.Dislike && isAnswer && voteRequest.UserId != targetId)
            {
                await client.PutAsJsonAsync("http://users.api:5005/api/Users/Score", new ModifyScore
                {
                    UserId = voteRequest.UserId,
                    Score = -1.5f
                });
            }

            vote = await votesRepository.CreateAsync(vote);
            return vote == null ? null : mapper.Map<VoteDto>(vote);
        }

        public async Task<bool> DeleteAsync(Guid id, VoteRequest voteRequest)
        {
            var client = httpClientFactory.CreateClient();
            string targetId;
            bool isAnswer;

            if (voteRequest.CommentId == null)
            {
                var responsePost = await client.GetAsync($"http://posts.api:5002/api/Posts/{voteRequest.PostId}");
                var content = await responsePost.Content.ReadFromJsonAsync<Post>();
                targetId = content.UserId;
                isAnswer = false;
            }
            else
            {
                var responseComment = await client.GetAsync($"http://comments.api:5003/api/Comments/{voteRequest.CommentId}");
                var content = await responseComment.Content.ReadFromJsonAsync<Comment>();
                targetId = content.UserId;
                isAnswer = true;
            }

            float score = GetScoreDelta(voteRequest.Type, isAnswer, isVoteRemoval: true);
            await client.PutAsJsonAsync("http://users.api:5005/api/Users/Score", new ModifyScore
            {
                UserId = targetId,
                Score = score
            });

            if (voteRequest.Type == Models.Type.Dislike && isAnswer && voteRequest.UserId != targetId)
            {
                await client.PutAsJsonAsync("http://users.api:5005/api/Users/Score", new ModifyScore
                {
                    UserId = voteRequest.UserId,
                    Score = 1.5f
                });
            }

            return await votesRepository.DeleteAsync(id);
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
            if (existingVote == null) return false;

            var client = httpClientFactory.CreateClient();
            string targetId;
            bool isAnswer;

            if (voteRequest.CommentId == null)
            {
                var responsePost = await client.GetAsync($"http://posts.api:5002/api/Posts/{voteRequest.PostId}");
                var content = await responsePost.Content.ReadFromJsonAsync<Post>();
                targetId = content.UserId;
                isAnswer = false;
            }
            else
            {
                var responseComment = await client.GetAsync($"http://comments.api:5003/api/Comments/{voteRequest.CommentId}");
                var content = await responseComment.Content.ReadFromJsonAsync<Comment>();
                targetId = content.UserId;
                isAnswer = true;
            }

            float undoScore = GetScoreDelta(existingVote.Type, isAnswer, isVoteRemoval: true);
            float newScore = GetScoreDelta(voteRequest.Type, isAnswer);

            await client.PutAsJsonAsync("http://users.api:5005/api/Users/Score", new ModifyScore
            {
                UserId = targetId,
                Score = undoScore + newScore
            });

            if (isAnswer && voteRequest.UserId != targetId)
            {
                float voterAdjustment = 0;

                if (existingVote.Type == Models.Type.Dislike)
                    voterAdjustment += 1.5f; // undo previous penalty

                if (voteRequest.Type == Models.Type.Dislike)
                    voterAdjustment -= 1.5f; // apply new penalty

                if (voterAdjustment != 0)
                {
                    await client.PutAsJsonAsync("http://users.api:5005/api/Users/Score", new ModifyScore
                    {
                        UserId = voteRequest.UserId,
                        Score = voterAdjustment
                    });
                }
            }
            var vote = mapper.Map<Vote>(voteRequest);
            vote.Id = id;
            return await votesRepository.UpdateAsync(vote);
        }

        public async Task<NrVotes> GetAllByPostIdAsync(Guid postId)
        {
            var votes = await votesRepository.GetAllByPostId(postId);
            if (votes == null) return null;
            return new NrVotes
            {
                Likes = votes.Count(v => v.Type == Models.Type.Like),
                Dislikes = votes.Count(v => v.Type == Models.Type.Dislike)
            };
        }

        public async Task<NrVotes> GetAllByCommentIdAsync(Guid commentId)
        {
            var votes = await votesRepository.GetAllByCommentId(commentId);
            if (votes == null) return null;
            return new NrVotes
            {
                Likes = votes.Count(v => v.Type == Models.Type.Like),
                Dislikes = votes.Count(v => v.Type == Models.Type.Dislike)
            };
        }

        public async Task<bool> DeleteAllByPostIdAsync(Guid postId)
        {
            var votes = await votesRepository.GetAllByPostId(postId);
            if (votes == null || votes.Count == 0) return false;

            var client = httpClientFactory.CreateClient();
            foreach (var vote in votes)
            {
                bool isAnswer = vote.CommentId != null;
                float score = GetScoreDelta(vote.Type, isAnswer, isVoteRemoval: true);

                string targetId;
                if (!isAnswer)
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

                await client.PutAsJsonAsync("http://users.api:5005/api/Users/Score", new ModifyScore
                {
                    UserId = targetId,
                    Score = score
                });
            }

            return await votesRepository.DeleteAllByPostId(postId);
        }

        public async Task<bool> DeleteAllByCommentIdAsync(Guid commentId)
        {
            var votes = await votesRepository.GetAllByCommentId(commentId);
            if (votes == null || votes.Count == 0) return false;

            var client = httpClientFactory.CreateClient();
            foreach (var vote in votes)
            {
                bool isAnswer = vote.CommentId != null;
                float score = GetScoreDelta(vote.Type, isAnswer, isVoteRemoval: true);

                string targetId;
                if (!isAnswer)
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

                await client.PutAsJsonAsync("http://users.api:5005/api/Users/Score", new ModifyScore
                {
                    UserId = targetId,
                    Score = score
                });
            }

            return await votesRepository.DeleteAllByCommentId(commentId);
        }

        public async Task<VoteDto> GetByUserIdAndTargetIdAsync(string userId, Guid? postId, Guid? commentId)
        {
            var vote = await votesRepository.GetByUserIdAndTargetIdAsync(userId, postId, commentId);
            return vote == null ? null : mapper.Map<VoteDto>(vote);
        }

        public async Task<bool> CheckUserVoteAsync(string userId, Guid? postId, Guid? commentId)
        {
            if (postId != null)
            {
                var post = await votesRepository.GetPostById((Guid)postId);
                if (post.UserId == userId) return false;
            }
            if (commentId != null)
            {
                var comment = await votesRepository.GetCommentById((Guid)commentId);
                if (comment.UserId == userId) return false;
            }
            return true;
        }
    }
}
