using System.Reflection;
using System.Text.Json;
using albionSCRAPERV2.Data;
using albionSCRAPERV2.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace albionSCRAPERV2.Services;

public class ItemImporter
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<ItemImporter> _logger;
    
    private const string ResourceName = "albionSCRAPERV2.Data.coscos.json";

    public ItemImporter(ApplicationDbContext context, ILogger<ItemImporter> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task ImportAsync()
    {
          try
        {
            var assembly = Assembly.GetExecutingAssembly();

            // Wczytaj strumień z zasobu osadzonego
            using Stream? stream = assembly.GetManifestResourceStream(ResourceName);
            if (stream == null)
            {
                _logger.LogError("Nie znaleziono zasobu osadzonego: {ResourceName}", ResourceName);
                return;
            }

            using var reader = new StreamReader(stream);
            string jsonContent = await reader.ReadToEndAsync();

            var items = JsonSerializer.Deserialize<List<JsonItem>>(jsonContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (items == null)
            {
                _logger.LogError("Błąd deserializacji danych z zasobu osadzonego.");
                return;
            }

            foreach (var jsonItem in items)
            {
                if (string.IsNullOrEmpty(jsonItem.UniqueName) ||
                    !jsonItem.LocalizedNames.ContainsKey("EN-US"))
                {
                    continue;
                }

                // Sprawdź, czy już istnieje
                var exists = await _context.Items.AnyAsync(i => i.UniqueName == jsonItem.UniqueName);
                if (!exists)
                {
                    var item = new Item
                    {
                        UniqueName = jsonItem.UniqueName,
                        Name = jsonItem.LocalizedNames["EN-US"],
                        DescriptionEN = jsonItem.LocalizedDescriptions.GetValueOrDefault("EN-US", string.Empty),
                    };

                    _context.Items.Add(item);
                }
            }

            await _context.SaveChangesAsync();
            _logger.LogInformation("Zasób osadzony został pomyślnie zaimportowany do bazy danych.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Błąd podczas importowania danych z zasobu osadzonego.");
            throw;
        }
    }

    private class JsonItem
    {
        public string UniqueName { get; set; } = string.Empty;
        public Dictionary<string, string> LocalizedNames { get; set; } = new();
        public Dictionary<string, string> LocalizedDescriptions { get; set; } = new();
    }
} 