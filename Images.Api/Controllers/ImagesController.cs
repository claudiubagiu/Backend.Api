using Images.Api.Models.Domain;
using Images.Api.Models.DTOs;
using Images.Api.Repositories.Interface;
using Images.Api.Services.Interface;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Images.Api.Services.Implementation;

namespace Images.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IImageService imageService;
        private readonly IMapper mapper;

        public ImagesController(IImageService imageService, IMapper mapper)
        {
            this.imageService = imageService;
            this.mapper = mapper;
        }

        // POST : /api/Images/Upload
        [HttpPost]
        [Route("Upload")]
        //[Authorize(Roles = "Admin, User")]
        public async Task<IActionResult> Upload([FromForm] UploadImageRequestDto request)
        {
            try
            {
                var imageDomainModel = await imageService.Upload(request);
                if (imageDomainModel != null)
                {
                    return Ok(imageDomainModel);
                }

                return BadRequest("Invalid file.");
            }
            catch (InvalidDataException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET : /api/Images
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var images = await imageService.GetAllAsync();
            if (images != null)
                return Ok(images);
            return BadRequest();
        }

        // GET : /api/Images/Post/{Id}
        [HttpGet]
        [Route("Post/{postId}")]
        public async Task<IActionResult> GetByPostId([FromRoute] Guid postId)
        {
            var image = await imageService.GetByPostIdAsync(postId);
            if (image != null)
                return Ok(image);
            return BadRequest();
        }

        // GET : /api/Images/Comment/{Id}
        [HttpGet]
        [Route("Comment/{commentId}")]
        public async Task<IActionResult> GetByCommentId([FromRoute] Guid commentId)
        {
            var image = await imageService.GetByCommentIdAsync(commentId);
            if (image != null)
                return Ok(image);
            return BadRequest();
        }

        // DELETE : /api/Images/Post/{Id}
        [HttpDelete]
        [Route("Post/{postId}")]
        public async Task<IActionResult> DeleteByPostId([FromRoute] Guid postId)
        {
            var result = await imageService.DeleteByPostIdAsync(postId);
            if (result == false)
                return BadRequest();
            return Ok(result);
        }

        // DELETE : /api/Images/Comment/{Id}
        [HttpDelete]
        [Route("Comment/{commentId}")]
        public async Task<IActionResult> DeleteByCommentId([FromRoute] Guid commentId)
        {
            var result = await imageService.DeleteByCommentIdAsync(commentId);
            if (result == false)
                return BadRequest();
            return Ok(result);
        }
    }
}
