using SQLite;
namespace albionSCRAPERV2.Models;

public class TrackedItem
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    [Indexed]
    public string ItemId { get; set; } = null!;
    public string Location { get; set; } = string.Empty;
    public int Quality { get; set; }
}