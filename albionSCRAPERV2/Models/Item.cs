using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace albionSCRAPERV2.Models;

public class Item
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ItemId { get; set; }

    public string UniqueName { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

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
        string name)
    {
        UniqueName = uniqueName;
        Name = name;
        
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
