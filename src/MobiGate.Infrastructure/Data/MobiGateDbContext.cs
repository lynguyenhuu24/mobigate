using Microsoft.EntityFrameworkCore;
using MobiGate.Domain.Entities;

namespace MobiGate.Infrastructure.Data;

public class MobiGateDbContext : DbContext
{
    public MobiGateDbContext(DbContextOptions<MobiGateDbContext> options) : base(options) { }

    public DbSet<Provider> Providers => Set<Provider>();
    public DbSet<Vehicle> Vehicles => Set<Vehicle>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("mobigate");

        modelBuilder.Entity<Provider>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).HasMaxLength(100).IsRequired();
            entity.Property(e => e.Slug).HasMaxLength(50).IsRequired();
            entity.HasIndex(e => e.Slug).IsUnique();
            entity.Property(e => e.BaseUrl).HasMaxLength(500).IsRequired();
        });

        modelBuilder.Entity<Vehicle>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).HasMaxLength(200).IsRequired();
            entity.Property(e => e.PlateNumber).HasMaxLength(20).IsRequired();
            entity.Property(e => e.Latitude).HasColumnType("decimal(10,7)");
            entity.Property(e => e.Longitude).HasColumnType("decimal(10,7)");
            entity.Property(e => e.PricePerMinute).HasColumnType("decimal(8,2)");
            entity.HasOne(e => e.Provider)
                  .WithMany()
                  .HasForeignKey(e => e.ProviderId);
            entity.HasIndex(e => e.Status);
            entity.HasIndex(e => new { e.Type, e.Status });
        });
    }
}
