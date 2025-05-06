using Auth.Models.Domain;

namespace Auth.Repositories.Interface
{
    public interface ITokenRepository
    {
        string CreateToken(ApplicationUser user, List<string> roles);
    }
}
