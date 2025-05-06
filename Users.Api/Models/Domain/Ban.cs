namespace Users.Api.Models.Domain
{
    public class Ban
    {
        public Guid Id { get; set; }
        public string Description { get; set; } = null!;
        public string UserId { get; set; }
        public ApplicationUser User { get; set; } = null!;
    }
}
