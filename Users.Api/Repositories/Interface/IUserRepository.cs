using Users.Api.Models.Domain;

namespace Users.Api.Repositories.Interface
{
    public interface IUserRepository
    {
        Task<List<ApplicationUser>> GetAllAsync();
        Task<ApplicationUser?> GetByIdAsync(string id);
        Task<float> ModifyScore(string id, float score);
    }
}
