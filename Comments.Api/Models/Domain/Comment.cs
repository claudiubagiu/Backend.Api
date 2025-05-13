namespace Comments.Api.Models.Domain
{
    public class Comment
    {
        public required Guid Id { get; set; }
        public required string Description { get; set; }
        public required DateTime CreatedAt { get; set; }
        public required string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public required Guid PostId { get; set; }
        public Guid? CommentId { get; set; }
        public ICollection<Comment> ChildrenComments { get; set; } = new List<Comment>();
        public List<Vote> Votes { get; set; } = new List<Vote>();
        public Image? Image { get; set; }
    }
}
