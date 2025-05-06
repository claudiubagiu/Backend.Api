using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Posts.Api.Models.Domain;

namespace Posts.Api.Data
{
    public class BackendDbContext : DbContext
    {
        public BackendDbContext(DbContextOptions<BackendDbContext> options) : base(options)
        {
        }

        public DbSet<Post> Post { get; set; }
        public DbSet<Tag> Tag { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Post>()
                   .Property(e => e.Status)
                   .HasConversion<string>();

            builder.Entity<Post>()
                   .HasMany(e => e.Tags)
                   .WithMany(e => e.Posts);
        }
    }
}
