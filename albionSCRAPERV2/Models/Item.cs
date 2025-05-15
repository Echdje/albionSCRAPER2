using System.Diagnostics.CodeAnalysis;

namespace albionSCRAPERV2.Models;

public class Item
{
    public required string UniqueName { get; set; }
    public required string LocalizationNameVariable { get; set; }
    public required string LocalizationDescriptionVariable { get; set; }
    public required Dictionary <string, string> LocalizedNames { get; set; }
    public required Dictionary <string, string> LocalizedDescriptions { get; set; }
    
    public int Tier => ExtractTier(UniqueName);
    public string Category => ExtractCategory(UniqueName);
    public string Subcategory => ExtractSubcategory(UniqueName);
    public string Faction => ExtractFaction(UniqueName);

    [SetsRequiredMembers]
    public Item(
        string uniqueName,
        string localizationNameVariable,
        string localizationDescriptionVariable,
        Dictionary<string, string> localizedNames,
        Dictionary<string, string> localizedDescriptions)
    {
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
            return int.Parse(uniqueName.Substring(1,1));
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
        string subcategoryName = "";
        var parts = uniqueName.Split('_');
        if (parts.Length > 2)
        {
            for (int i = 2; i < parts.Length - 1; i++)
            {
                subcategoryName += parts[i];
                subcategoryName += " ";
            }
        }else{
            return string.Empty;
        }

        return subcategoryName;
    }
    
    private string ExtractFaction(string uniqueName)
    {
        var parts = uniqueName.Split('_');
        return parts[^1];
    }

}