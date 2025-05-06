namespace Posts.Api.Models.Domain
{
    public class Tag
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public List<Post> Posts { get; set; } = new List<Post>();
    }
}
