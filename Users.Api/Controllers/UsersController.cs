using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Users.Api.Models.DTOs;
using Users.Api.Services.Interface;

namespace Users.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService userService;
        private readonly IBanService banService;

        public UsersController(IUserService userService, IBanService banService)
        {
            this.userService = userService;
            this.banService = banService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<List<ApplicationUserDto>>> GetAll()
        {
            var users = await userService.GetAllAsync();
            if (users != null)
            {
                return Ok(users);
            }
            return NotFound();
        }

        [HttpGet]
        [Route("{id}")]
        //[Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApplicationUserDto>> GetById(string id)
        {
            var user = await userService.GetByIdAsync(id);
            if (user != null)
            {
                return Ok(user);
            }
            return NotFound();
        }

        [HttpPost]
        [Route("ban")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<bool>> BanUser([FromBody] AddBanRequest addBanRequest)
        {
            var result = await banService.BanUserAsync(addBanRequest);
            if (result)
            {
                return Ok(result);
            }
            return NotFound();
        }

        [HttpDelete]
        [Route("unban/{userId}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<bool>> UnbanUser(string userId)
        {
            var result = await banService.UnbanUserAsync(userId);
            if (result)
            {
                return Ok(result);
            }
            return NotFound();
        }

        [HttpGet]
        [Route("isBanned/{userEmail}")]
        public async Task<ActionResult<bool>> IsUserBanned([FromRoute] string userEmail)
        {
            var result = await banService.IsUserBannedAsync(userEmail);
            return Ok(result);
        }

        [HttpPut]
        [Route("Score")]
        public async Task<ActionResult<int>> ModifyScore([FromBody] ModifyScore modifyScore)
        {
            var result = await userService.ModifyScore(modifyScore);
            if (result > 0)
            {
                return Ok(result);
            }
            return NotFound();
        }
    }
}
