using Microsoft.EntityFrameworkCore;
using albionSCRAPERV2.Models;

namespace albionSCRAPERV2.Data;

public class ApplicationDbContext : DbContext
{
    public DbSet<Item> Items { get; set; }
    public DbSet<TrackedItem> TrackedItems { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Item>(entity =>
        {
            entity.HasKey(e => e.ItemId);
            entity.Property(e => e.UniqueName).IsRequired();
            entity.Property(e => e.LocalizationNameVariable).IsRequired();
            entity.Property(e => e.LocalizationDescriptionVariable).IsRequired();
            entity.Property(e => e.LocalizedNames);
        });

        modelBuilder.Entity<TrackedItem>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.ItemId).IsRequired();
            entity.Property(e => e.Location).IsRequired();
            entity.Property(e => e.Quality).IsRequired();

            entity.HasOne<Item>()
                .WithMany()
                .HasForeignKey(e => e.ItemId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
} 