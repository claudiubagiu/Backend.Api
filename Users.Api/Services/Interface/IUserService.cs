using Users.Api.Models.Domain;
using Users.Api.Models.DTOs;

namespace Users.Api.Services.Interface
{
    public interface IUserService
    {
        Task<List<ApplicationUserDto>> GetAllAsync();
        Task<ApplicationUserDto?> GetByIdAsync(string id);
        Task<int> ModifyScore(ModifyScore modifyScore);
    }
}
