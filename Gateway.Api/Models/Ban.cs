namespace Gateway.Api.Models
{
    public class Ban
    {
        public required Guid Id { get; set; }
        public string Description { get; set; } = null!;
        public required string UserId { get; set; }
        public ApplicationUser User { get; set; } = null!;
    }
}
