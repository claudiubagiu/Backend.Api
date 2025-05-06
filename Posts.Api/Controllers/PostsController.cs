using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Posts.Api.Models.Domain;
using Posts.Api.Models.DTOs;
using Posts.Api.Repositories.Interface;
using Posts.Api.Services.Interface;
using System.Security.Claims;

namespace Posts.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly IPostService postService;

        public PostsController(IPostService postService)
        {
            this.postService = postService;
        }

        // GET : api/Posts
        [HttpGet]
        public async Task<ActionResult<List<PostDto>>> GetAll()
        {
            var posts = await postService.GetAllAsync();
            if (posts != null)
            {
                return Ok(posts);
            }
            return NotFound();
        }

        // GET : api/Posts/{id}
        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<Post>> GetById(Guid id)
        {
            var post = await postService.GetByIdAsync(id);
            if (post != null)
            {
                return Ok(post);
            }
            return NotFound();
        }

        // POST : api/Posts
        [HttpPost]
        [Authorize(Roles = "Admin, User")]
        public async Task<ActionResult<Post>> Create([FromBody] AddPostRequestDto addPostRequestDto)
        {
            var userIdClaim = HttpContext.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Unauthorized("User ID is missing from token.");
            }

            var userId = userIdClaim.Value;
            DateTime dateTime = DateTime.Now;
            var postDto = await postService.CreateAsync(addPostRequestDto, dateTime, userId);
            if (postDto != null)
                return CreatedAtAction(nameof(GetById), new { id = postDto.Id }, postDto);
            return BadRequest();
        }

        // PUT : api/Posts/{id}
        [HttpPut]
        [Authorize(Roles = "Admin, User")]
        [Route("{id}")]
        public async Task<ActionResult<Post>> Update([FromRoute] Guid id, [FromBody] UpdatePostRequestDto updatePostRequestDto)
        {
            var userIdClaim = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            var userRoleClaims = HttpContext.User.FindAll(ClaimTypes.Role).Select(r => r.Value).ToList();

            if (userIdClaim == null)
                return Unauthorized("User ID is missing from token.");

            var userId = userIdClaim.Value;

            var postDto = await postService.UpdateAsync(id, updatePostRequestDto, userId, userRoleClaims);

            if (postDto != null)
                return Ok(postDto);
            return BadRequest();
        }

        // DELETE : api/Posts/{id}
        [HttpDelete]
        [Authorize(Roles = "Admin, User")]
        [Route("{id}")]
        public async Task<ActionResult<Boolean>> Delete(Guid id)
        {
            var userIdClaim = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            var userRoleClaims = HttpContext.User.FindAll(ClaimTypes.Role).Select(r => r.Value).ToList();

            if (userIdClaim == null)
                return Unauthorized("User ID is missing from token.");

            var userId = userIdClaim.Value;

            var result = await postService.DeleteAsync(id, userId, userRoleClaims);
            if (result)
                return Ok(result);
            return NotFound();
        }
    }
}
