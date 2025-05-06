namespace Users.Api.Models.DTOs
{
    public class AddBanRequest
    {
        public string UserId { get; set; } = null!;
        public string Description { get; set; } = null!;
    }
}
