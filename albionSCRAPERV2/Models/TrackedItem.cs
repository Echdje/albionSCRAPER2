using Microsoft.EntityFrameworkCore;

namespace albionSCRAPERV2.Models;

public class TrackedItem
{
    public int Id { get; set; }
    public string ItemId { get; set; } = null!;
    public string Location { get; set; } = string.Empty;
    public int Quality { get; set; }

    public virtual Item? Item { get; set; }
}