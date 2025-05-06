namespace Auth.Models.DTOs
{
    public class LoginResponseDto
    {
        public required string JwtToken { get; set; }
        public required string Email { get; set; }
        public required List<string> Roles { get; set; }
    }
}
