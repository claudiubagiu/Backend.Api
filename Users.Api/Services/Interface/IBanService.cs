using Users.Api.Models.DTOs;

namespace Users.Api.Services.Interface
{
    public interface IBanService
    {
        Task<bool> IsUserBannedAsync(string userEmail);
        Task<bool> BanUserAsync(AddBanRequest addBanRequest);
        Task<bool> UnbanUserAsync(string userId);
    }
}
