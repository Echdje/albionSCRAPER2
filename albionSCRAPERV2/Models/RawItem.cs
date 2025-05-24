namespace albionSCRAPERV2.Models;

public class RawItem
{
    public string UniqueName { get; set; } = string.Empty;
    public Dictionary<string, string> LocalizedNames { get; set; } = new();
}