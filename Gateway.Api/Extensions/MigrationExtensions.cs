using Gateway.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace Gateway.Api.Extensions
{
    public static class MigrationExtensions
    {
        public static void ApplyMigrations(this IApplicationBuilder app)
        {
            using IServiceScope scope = app.ApplicationServices.CreateScope();

            var db = scope.ServiceProvider.GetRequiredService<BackendDbContext>();

            if (!db.Database.CanConnect())
            {
                db.Database.Migrate(); // Only migrate if DB is not reachable
            }
            else
            {
                Console.WriteLine("Database already exists and is reachable. Skipping migration.");
            }
            
        }
    }
}
