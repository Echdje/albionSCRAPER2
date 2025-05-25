using Microsoft.EntityFrameworkCore;

namespace albionSCRAPERV2.Models;

public class TrackedItem
{
    public int Id { get; set; }
    public int ItemId { get; set; }
    public string Location { get; set; } = string.Empty;
    public int Quality { get; set; }

    public virtual Item? Item { get; set; }
}