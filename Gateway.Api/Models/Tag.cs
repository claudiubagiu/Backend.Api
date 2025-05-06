namespace Gateway.Api.Models
{
    public class Tag
    {
        public required Guid Id { get; set; }
        public required string Name { get; set; }
        public List<Post> Posts { get; } = [];
    }
}
