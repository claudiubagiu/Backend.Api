using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Images.Api.Models.Domain;

namespace Images.Api.Data
{
    public class BackendDbContext : IdentityDbContext
    {
        public BackendDbContext(DbContextOptions<BackendDbContext> options) : base(options)
        {
        }

        public DbSet<Image> Image { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
