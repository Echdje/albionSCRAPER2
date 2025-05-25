using Microsoft.Extensions.Logging;
using albionSCRAPERV2.Data;
using albionSCRAPERV2.Services;

namespace albionSCRAPERV2;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

#if DEBUG
        builder.Logging.AddDebug();
#endif

        // Configure database
        builder.Services.ConfigureDatabase();
        
        // Register ItemImporter service
        builder.Services.AddScoped<ItemImporter>();

        var app = builder.Build();

        // Run database initialization and data import
        using (var scope = app.Services.CreateScope())
        {
            var importer = scope.ServiceProvider.GetRequiredService<ItemImporter>();
            Task.Run(async () =>
            {
                try
                {
                    await importer.ImportAsync();
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error importing items: {ex}");
                }
            }).Wait();
        }

        return app;
    }
}