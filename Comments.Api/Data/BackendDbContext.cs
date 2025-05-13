using Comments.Api.Models.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Posts.Api.Models.Domain;
using System.Reflection.Emit;
using System.Reflection.Metadata;

namespace Comments.Api.Data
{
    public class BackendDbContext : IdentityDbContext<ApplicationUser>
    {
        public BackendDbContext(DbContextOptions<BackendDbContext> options) : base(options)
        {
        }

        public DbSet<Comment> Comment { get; set; }
        public DbSet<Post> Post { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Post>()
               .Property(e => e.Status)
               .HasConversion<string>();
        }
    }
}
