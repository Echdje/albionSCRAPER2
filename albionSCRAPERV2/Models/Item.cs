using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace albionSCRAPERV2.Models;

public class Item
{
    [Key]
    public string ItemId { get; set; } = string.Empty;

    public string UniqueName { get; set; } = string.Empty;
    public string LocalizationNameVariable { get; set; } = string.Empty;
    public string LocalizationDescriptionVariable { get; set; } = string.Empty;

    // EF + SQLite nie wspierają Dictionary<> - musimy je serializować
    public string LocalizedNamesJson { get; set; } = "{}";
    public string LocalizedDescriptionsJson { get; set; } = "{}";

    [NotMapped]
    public Dictionary<string, string> LocalizedNames
    {
        get => JsonSerializer.Deserialize<Dictionary<string, string>>(LocalizedNamesJson) ?? new();
        set => LocalizedNamesJson = JsonSerializer.Serialize(value);
    }

    [NotMapped]
    public Dictionary<string, string> LocalizedDescriptions
    {
        get => JsonSerializer.Deserialize<Dictionary<string, string>>(LocalizedDescriptionsJson) ?? new();
        set => LocalizedDescriptionsJson = JsonSerializer.Serialize(value);
    }

    [NotMapped]
    public int Tier => ExtractTier(UniqueName);

    [NotMapped]
    public string Category => ExtractCategory(UniqueName);

    [NotMapped]
    public string Subcategory => ExtractSubcategory(UniqueName);

    [NotMapped]
    public string Faction => ExtractFaction(UniqueName);

    // EF wymaga konstruktora bezparametrowego
    public Item() {}

    [SetsRequiredMembers]
    public Item(
        string uniqueName,
        string localizationNameVariable,
        string localizationDescriptionVariable,
        Dictionary<string, string> localizedNames,
        Dictionary<string, string> localizedDescriptions)
    {
        ItemId = uniqueName;
        UniqueName = uniqueName;
        LocalizationNameVariable = localizationNameVariable;
        LocalizationDescriptionVariable = localizationDescriptionVariable;
        LocalizedNames = localizedNames;
        LocalizedDescriptions = localizedDescriptions;
    }

    private int ExtractTier(string uniqueName)
    {
        if (uniqueName.StartsWith("T") && Char.IsDigit(uniqueName[1]))
        {
            return int.Parse(uniqueName.Substring(1, 1));
        }

        return 0;
    }

    private string ExtractCategory(string uniqueName)
    {
        var parts = uniqueName.Split('_');
        return parts.Length > 1 ? parts[1] : string.Empty;
    }

    private string ExtractSubcategory(string uniqueName)
    {
        var parts = uniqueName.Split('_');
        if (parts.Length <= 2) return string.Empty;

        return string.Join(" ", parts.Skip(2).Take(parts.Length - 3));
    }

    private string ExtractFaction(string uniqueName)
    {
        var parts = uniqueName.Split('_');
        return parts[^1];
    }
}
