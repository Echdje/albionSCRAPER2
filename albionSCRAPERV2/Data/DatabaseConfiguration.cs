using Microsoft.EntityFrameworkCore;
using albionSCRAPERV2.Services;

namespace albionSCRAPERV2.Data;

public static class DatabaseConfiguration
{
    public static IServiceCollection AddDatabaseServices(this IServiceCollection services)
    {
        var dbPath = Path.Combine(FileSystem.AppDataDirectory, "albion_items.db");
        
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlite($"Data Source={dbPath}"));

        services.AddScoped<DatabaseService>();

        return services;
    }
} 