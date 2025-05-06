using AutoMapper.Configuration.Annotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Posts.Api.Models.DTOs;
using Posts.Api.Services.Interface;

namespace Posts.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagsController : ControllerBase
    {
        private readonly ITagService tagService;

        public TagsController(ITagService tagService)
        {
            this.tagService = tagService;
        }

        // GET : api/Tags
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var tags = await tagService.GetAllAsync();
            if (tags == null)
                return NotFound();
            return Ok(tags);
        }

        // GET : api/Tags/{id}
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var tag = await tagService.GetByIdAsync(id);
            if (tag == null) return NotFound();
            return Ok(tag);
        }

        // POST : api/Tags
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] AddTagRequestDto addTagRequestDto)
        {
            var tag = await tagService.CreateAsync(addTagRequestDto);
            if (tag == null) return BadRequest();
            return Ok(tag);
        }

        // DELETE : api/Tags/{id}
        [HttpDelete]
        [Route("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var result = await tagService.DeleteAsync(id);
            if (result)
                return Ok(result);
            return NotFound();
        }
    }
}
