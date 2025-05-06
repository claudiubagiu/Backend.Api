using Users.Api.Models.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Users.Api.Data
{
    public class BackendDbContext : IdentityDbContext<ApplicationUser>
    {
        public BackendDbContext(DbContextOptions<BackendDbContext> options) : base(options)
        {
        }
        public DbSet<Ban> Ban { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
