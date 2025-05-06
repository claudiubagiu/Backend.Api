using Microsoft.EntityFrameworkCore;
using Users.Api.Data;
using Users.Api.Models.Domain;
using Users.Api.Repositories.Interface;

namespace Users.Api.Repositories.Implementation
{
    public class BanRepository : IBanRepository
    {
        private readonly BackendDbContext dbContext;

        public BanRepository(BackendDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<bool> BanUserAsync(Ban ban)
        {
            await dbContext.Ban.AddAsync(ban);
            await dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> IsUserBannedAsync(string userEmail)
        {
            var ban = await dbContext.Ban.FirstOrDefaultAsync(b => b.User.Email == userEmail);
            if (ban == null)
                return false;
            return true;
        }

        public async Task<bool> UnbanUserAsync(string userId)
        {
            var ban = await dbContext.Ban.FirstOrDefaultAsync(b => b.UserId == userId);
            if (ban == null)
                return false;
            dbContext.Ban.Remove(ban);
            await dbContext.SaveChangesAsync();
            return true;
        }
    }
}
