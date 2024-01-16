using BaseTest.Repository.Context;
using Microsoft.EntityFrameworkCore;

namespace BaseTest.API;

public static class DbMigrationExtension
{
    public static void UseDbMigration(this IApplicationBuilder app)
    {
        using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>()!.CreateScope())
        {
            var context = serviceScope.ServiceProvider.GetRequiredService<AppDbContext>();
            context.Database.Migrate();
        }
    }
}