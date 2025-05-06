using Gateway.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace Gateway.Api.Extensions
{
    public static class MigrationExtensions
    {
        public static void ApplyMigrations(this IApplicationBuilder app)
        {
            using IServiceScope scope = app.ApplicationServices.CreateScope();

            using BackendDbContext dbContext =
                scope.ServiceProvider.GetRequiredService<BackendDbContext>();

            dbContext.Database.Migrate();
        }
    }
}
