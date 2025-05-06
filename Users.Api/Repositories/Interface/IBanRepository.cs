using Users.Api.Models.Domain;

namespace Users.Api.Repositories.Interface
{
    public interface IBanRepository
    {
        Task<bool> IsUserBannedAsync(string userEmail);
        Task<bool> BanUserAsync(Ban ban);
        Task<bool> UnbanUserAsync(string userId);
    }
}
