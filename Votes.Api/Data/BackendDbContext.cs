using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using Votes.Api.Models;
using Votes.Api.Models.Domain;

namespace Votes.Api.Data
{
    public class BackendDbContext : IdentityDbContext
    {
        public BackendDbContext(DbContextOptions<BackendDbContext> options) : base(options)
        {
        }
        public DbSet<Vote> Vote { get; set; }
        public DbSet<Post> Post { get; set; }
        public DbSet<Comment> Comment { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Vote>()
                .Property(v => v.Type)
                .HasConversion<string>();
        }
    }
}
