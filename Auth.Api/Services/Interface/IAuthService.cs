using Auth.Models.DTOs;

namespace Auth.Services.Interface
{
    public interface IAuthService
    {
        Task<LoginResponseDto?> Login(LoginRequestDto loginRequestDto);
        Task<LoginResponseDto?> Register(RegisterRequestDto registerRequestDto);
        Task<Boolean> CheckBan(string userId);
    }
}
