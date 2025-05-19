using Microsoft.EntityFrameworkCore;
using albionSCRAPERV2.Data;
using albionSCRAPERV2.Models;

namespace albionSCRAPERV2.Services;

public class DatabaseService
{
    private readonly ApplicationDbContext _context;

    public DatabaseService(ApplicationDbContext context)
    {
        _context = context;
    }

    // Item operations
    public async Task<List<Item>> GetItemsAsync()
    {
        return await _context.Items.ToListAsync();
    }

    public async Task<Item?> GetItemAsync(string itemId)
    {
        return await _context.Items.FindAsync(itemId);
    }

    public async Task<int> SaveItemAsync(Item item)
    {
        if (await _context.Items.FindAsync(item.ItemId) == null)
        {
            _context.Items.Add(item);
        }
        else
        {
            _context.Items.Update(item);
        }
        return await _context.SaveChangesAsync();
    }

    public async Task<int> DeleteItemAsync(Item item)
    {
        _context.Items.Remove(item);
        return await _context.SaveChangesAsync();
    }

    // TrackedItem operations
    public async Task<List<TrackedItem>> GetTrackedItemsAsync()
    {
        return await _context.TrackedItems
            .Include(t => t.Item)
            .ToListAsync();
    }

    public async Task<TrackedItem?> GetTrackedItemAsync(int id)
    {
        return await _context.TrackedItems
            .Include(t => t.Item)
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<int> SaveTrackedItemAsync(TrackedItem trackedItem)
    {
        if (trackedItem.Id == 0)
        {
            _context.TrackedItems.Add(trackedItem);
        }
        else
        {
            _context.TrackedItems.Update(trackedItem);
        }
        return await _context.SaveChangesAsync();
    }

    public async Task<int> DeleteTrackedItemAsync(TrackedItem trackedItem)
    {
        _context.TrackedItems.Remove(trackedItem);
        return await _context.SaveChangesAsync();
    }

    // Get tracked items with their associated items
    public async Task<List<(TrackedItem TrackedItem, Item Item)>> GetTrackedItemsWithDetailsAsync()
    {
        var trackedItems = await _context.TrackedItems
            .Include(t => t.Item)
            .ToListAsync();

        return trackedItems
            .Where(t => t.Item != null)
            .Select(t => (t, t.Item!))
            .ToList();
    }
} 