using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Users.Api.Data;
using Users.Api.Models.Domain;
using Users.Api.Repositories.Interface;

namespace Users.Api.Repositories.Implementation
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<ApplicationUser> userManager;
        public UserRepository(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }

        public async Task<ApplicationUser?> GetByIdAsync(string id)
        {
            return await userManager.Users.Include(u => u.Ban).FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<List<ApplicationUser>> GetAllAsync()
        {
            return await userManager.Users.Include(u => u.Ban).ToListAsync();
        }

        public async Task<float> ModifyScore(string id, float score)
        {
            var user = await userManager.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
                return 0;
            user.Score += score;
            await userManager.UpdateAsync(user);
            return user.Score;
        }
    }
}
