using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using albionSCRAPERV2.Services;

namespace albionSCRAPERV2.Data;

public static class DatabaseConfiguration
{
    public static string DbPath => Path.Combine(
        FileSystem.AppDataDirectory,
        "albionItems.db");

    public static IServiceCollection ConfigureDatabase(this IServiceCollection services)
    {
        System.Diagnostics.Debug.WriteLine($"[DatabaseConfiguration] SQLite DB Path: {DbPath}");
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlite($"Data Source={DbPath}"));

        services.AddScoped<DatabaseService>();

        return services;
    }
} 