namespace Users.Api.Models.DTOs
{
    public class BanDto
    {
        public required Guid Id { get; set; }
        public string Description { get; set; } = null!;

    }
}
