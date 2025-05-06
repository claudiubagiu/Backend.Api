using Comments.Api.Models.DTOs;
using Comments.Api.Repositories.Interface;
using Comments.Api.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Comments.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly ICommentService commentService;

        public CommentsController(ICommentService commentService)
        {
            this.commentService = commentService;
        }

        // GET : api/Comments
        [HttpGet]
        public async Task<ActionResult<List<CommentDto>>> GetAll()
        {
            var comments = await commentService.GetAllAsync();
            if (comments != null)
            {
                return Ok(comments);
            }
            return NotFound();
        }

        // GET : api/Comments/{id}
        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<CommentDto>> GetById(Guid id)
        {
            var comment = await commentService.GetByIdAsync(id);
            if (comment != null)
            {
                return Ok(comment);
            }
            return NotFound();
        }

        // POST : api/Comments
        [HttpPost]
        [Authorize(Roles = "Admin, User")]
        public async Task<ActionResult<CommentDto>> Create([FromBody] AddCommentRequestDto addCommentRequestDto)
        {
            var userIdClaim = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Unauthorized("User ID is missing from token.");
            }
            var userId = userIdClaim.Value;
            DateTime dateTime = DateTime.Now;
            var commentDto = await commentService.CreateAsync(addCommentRequestDto, dateTime, userId);
            if (commentDto != null)
            {
                return Ok(commentDto);
            }
            return BadRequest();
        }

        // PUT : api/Comments/{id}
        [HttpPut]
        [Authorize(Roles = "Admin, User")]
        [Route("{id}")]
        public async Task<ActionResult<CommentDto>> Update([FromRoute] Guid id, [FromBody] UpdateCommentRequestDto updateCommentRequestDto)
        {
            var userIdClaim = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            var userRoleClaims = HttpContext.User.FindAll(ClaimTypes.Role).Select(r => r.Value).ToList();

            if (userIdClaim == null)
            {
                return Unauthorized("User ID is missing from token.");
            }
            var userId = userIdClaim.Value;
            var commentDto = await commentService.UpdateAsync(id, updateCommentRequestDto, userId, userRoleClaims);
            if (commentDto != null)
            {
                return Ok(commentDto);
            }
            return BadRequest();
        }

        // DELETE : api/Comments/{id}
        [HttpDelete]
        [Authorize(Roles = "Admin, User")]
        [Route("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var userIdClaim = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Unauthorized("User ID is missing from token.");
            }
            var userId = userIdClaim.Value;
            var userRoleClaims = HttpContext.User.FindAll(ClaimTypes.Role).Select(c => c.Value).ToList();
            var result = await commentService.DeleteAsync(id, userId, userRoleClaims);
            if (result)
            {
                return Ok();
            }
            return BadRequest();
        }

        // GET : api/Comments/Post/{postId}
        [HttpGet]
        [Route("Post/{postId}")]
        public async Task<ActionResult<List<CommentDto>>> GetAllByPostId([FromRoute] Guid postId)
        {
            var comments = await commentService.GetAllByPostIdAsync(postId);
            if (comments != null)
            {
                return Ok(comments);
            }
            return NotFound();
        }
    }
}
