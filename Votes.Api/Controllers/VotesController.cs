using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Votes.Api.Models.DTOs;
using Votes.Api.Services.Interface;

namespace Votes.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VotesController : ControllerBase
    {
        private readonly IVotesService votesService;

        public VotesController(IVotesService votesService)
        {
            this.votesService = votesService;
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var vote = await votesService.GetByIdAsync(id);
            if (vote == null)
                return NotFound();
            return Ok(vote);
        }

        [HttpGet]
        [Route("post/{postId}")]
        public async Task<IActionResult> GetAllByPostId([FromRoute] Guid postId)
        {
            var result = await votesService.GetAllByPostIdAsync(postId);
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        [HttpGet]
        [Route("comment/{commentId}")]
        public async Task<IActionResult> GetAllByCommentId([FromRoute] Guid commentId)
        {
            var result = await votesService.GetAllByCommentIdAsync(commentId);
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        [HttpDelete]
        [Route("post/{postId}")]
        public async Task<IActionResult> DeleteAllByPostId([FromRoute] Guid postId)
        {
            var result = await votesService.DeleteAllByPostIdAsync(postId);
            if (!result)
                return NotFound();
            return Ok(true);
        }

        [HttpDelete]
        [Route("comment/{commentId}")]
        public async Task<IActionResult> DeleteAllByCommentId([FromRoute] Guid commentId)
        {
            var result = await votesService.DeleteAllByCommentIdAsync(commentId);
            if (!result)
                return NotFound();
            return Ok(true);
        }

        [HttpPost]
        [Authorize(Roles = "Admin, User")]
        public async Task<IActionResult> Vote([FromBody] VoteRequest voteRequest)
        {
            var existingVote = await votesService.GetByUserIdAndTargetIdAsync(voteRequest.UserId, voteRequest.PostId, voteRequest.CommentId);
            bool validVote = await votesService.CheckUserVoteAsync(voteRequest.UserId, voteRequest.PostId, voteRequest.CommentId);
            if (!validVote)
                return BadRequest("Invalid vote request.");
            if (existingVote == null)
            {
                var result = await votesService.CreateAsync(voteRequest);
                if (result == null)
                    return BadRequest();
                return Ok(result);
            }

            if (existingVote.Type == voteRequest.Type)
            {
                var result = await votesService.DeleteAsync(existingVote.Id, voteRequest);
                if (!result)
                    return NotFound();
                return Ok(true);
            }
            else
            {
                var result = await votesService.UpdateAsync(existingVote.Id, voteRequest);
                if (!result)
                    return NotFound();
                return Ok(true);
            }
        }


        [HttpGet("user-vote")]
        public async Task<IActionResult> GetUserVote([FromQuery] string userId, [FromQuery] Guid? postId, [FromQuery] Guid? commentId)
        {
            if (string.IsNullOrEmpty(userId))
                return BadRequest("UserId is required.");

            var vote = await votesService.GetByUserIdAndTargetIdAsync(userId, postId, commentId);

            if (vote == null)
                return Ok(null);

            return Ok(vote);
        }

    }
}
