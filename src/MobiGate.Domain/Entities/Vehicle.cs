namespace MobiGate.Domain.Entities;

public enum VehicleType
{
    Scooter,
    Bike,
    Car
}

public enum VehicleStatus
{
    Available,
    Reserved,
    InMaintenance
}

public class Vehicle
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string PlateNumber { get; set; } = string.Empty;
    public VehicleType Type { get; set; }
    public VehicleStatus Status { get; set; } = VehicleStatus.Available;
    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }
    public decimal PricePerMinute { get; set; }
    public Guid ProviderId { get; set; }
    public Provider Provider { get; set; } = null!;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
