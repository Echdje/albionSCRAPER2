using System.Reflection;
using System.Text.Json;
using albionSCRAPERV2.Models;

namespace albionSCRAPERV2.Services;

public class ItemDataLoader
{
    private const string ResourceName = "albionSCRAPERV2.Data.coscos.json";

    public async Task<List<Item>> LoadItemsAsync()
    {
        try
        {
            var assembly = Assembly.GetExecutingAssembly();
            using Stream? stream = assembly.GetManifestResourceStream(ResourceName);
            if (stream == null)
                throw new FileNotFoundException("Nie znaleziono pliku items.json jako zasobu osadzonego.");

            using var reader = new StreamReader(stream);
            string json = await reader.ReadToEndAsync();

            var rawItems = JsonSerializer.Deserialize<List<RawItem>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (rawItems is null)
                return new();

            var mapped = rawItems
                .Where(i => !string.IsNullOrWhiteSpace(i.UniqueName))
                .Select(i => new Item(
                    i.UniqueName,
                    i.LocalizedNames.TryGetValue("EN-US", out var name) ? name : i.UniqueName))
                .ToList();

            return mapped;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Błąd podczas ładowania danych: {ex.Message}");
            return new List<Item>();
        }
    }  
}