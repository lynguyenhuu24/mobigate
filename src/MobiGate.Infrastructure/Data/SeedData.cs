using Microsoft.EntityFrameworkCore;
using MobiGate.Domain.Entities;
using MobiGate.Domain.Enums;

namespace MobiGate.Infrastructure.Data;

public static class SeedData
{
    public static readonly Provider[] Providers =
    [
        new() { Id = Guid.Parse("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), Name = "ScooterCo",  Slug = "scooterco",  BaseUrl = "https://api.scooterco.mock", IsActive = true },
        new() { Id = Guid.Parse("b2c3d4e5-f6a7-8901-bcde-f12345678901"), Name = "BikeNow",    Slug = "bikenow",    BaseUrl = "https://api.bikenow.mock",   IsActive = true },
        new() { Id = Guid.Parse("c3d4e5f6-a7b8-9012-cdef-123456789012"), Name = "TaxiGo",     Slug = "taxigo",     BaseUrl = "https://api.taxigo.mock",    IsActive = true },
    ];

    private static readonly Guid ScooterCoId = Guid.Parse("a1b2c3d4-e5f6-7890-abcd-ef1234567890");
    private static readonly Guid BikeNowId = Guid.Parse("b2c3d4e5-f6a7-8901-bcde-f12345678901");
    private static readonly Guid TaxiGoId = Guid.Parse("c3d4e5f6-a7b8-9012-cdef-123456789012");

    public static readonly Vehicle[] Vehicles =
    [
        // ScooterCo — 5 scooters
        new() { Id = Guid.Parse("d4e5f6a7-b8c9-0123-defa-234567890123"), Name = "Scooter X1",  PlateNumber = "SC-001", Type = VehicleType.Scooter, Status = VehicleStatus.Available,  Latitude = 10.8231m,  Longitude = 106.6297m, PricePerMinute = 0.15m, ProviderId = ScooterCoId },
        new() { Id = Guid.Parse("e5f6a7b8-c9d0-1234-efab-345678901234"), Name = "Scooter X2",  PlateNumber = "SC-002", Type = VehicleType.Scooter, Status = VehicleStatus.Available,  Latitude = 10.8242m,  Longitude = 106.6310m, PricePerMinute = 0.15m, ProviderId = ScooterCoId },
        new() { Id = Guid.Parse("f6a7b8c9-d0e1-2345-fabc-456789012345"), Name = "Scooter X3",  PlateNumber = "SC-003", Type = VehicleType.Scooter, Status = VehicleStatus.Available,  Latitude = 10.8253m,  Longitude = 106.6325m, PricePerMinute = 0.12m, ProviderId = ScooterCoId },
        new() { Id = Guid.Parse("a7b8c9d0-e1f2-3456-abcd-567890123456"), Name = "Scooter X4",  PlateNumber = "SC-004", Type = VehicleType.Scooter, Status = VehicleStatus.InMaintenance, Latitude = 10.8264m,  Longitude = 106.6338m, PricePerMinute = 0.10m, ProviderId = ScooterCoId },
        new() { Id = Guid.Parse("b8c9d0e1-f2a3-4567-bcde-678901234567"), Name = "Scooter X5",  PlateNumber = "SC-005", Type = VehicleType.Scooter, Status = VehicleStatus.Available,  Latitude = 10.8275m,  Longitude = 106.6351m, PricePerMinute = 0.18m, ProviderId = ScooterCoId },

        // BikeNow — 5 bikes
        new() { Id = Guid.Parse("c9d0e1f2-a3b4-5678-cdef-789012345678"), Name = "Bike B1",   PlateNumber = "BN-001", Type = VehicleType.Bike, Status = VehicleStatus.Available,  Latitude = 10.8200m,  Longitude = 106.6280m, PricePerMinute = 0.08m, ProviderId = BikeNowId },
        new() { Id = Guid.Parse("d0e1f2a3-b4c5-6789-defa-890123456789"), Name = "Bike B2",   PlateNumber = "BN-002", Type = VehicleType.Bike, Status = VehicleStatus.Available,  Latitude = 10.8211m,  Longitude = 106.6293m, PricePerMinute = 0.08m, ProviderId = BikeNowId },
        new() { Id = Guid.Parse("e1f2a3b4-c5d6-7890-efab-901234567890"), Name = "Bike B3",   PlateNumber = "BN-003", Type = VehicleType.Bike, Status = VehicleStatus.Reserved,   Latitude = 10.8222m,  Longitude = 106.6306m, PricePerMinute = 0.07m, ProviderId = BikeNowId },
        new() { Id = Guid.Parse("f2a3b4c5-d6e7-8901-fabc-012345678901"), Name = "Bike B4",   PlateNumber = "BN-004", Type = VehicleType.Bike, Status = VehicleStatus.Available,  Latitude = 10.8233m,  Longitude = 106.6319m, PricePerMinute = 0.09m, ProviderId = BikeNowId },
        new() { Id = Guid.Parse("a3b4c5d6-e7f8-9012-abcd-123456789012"), Name = "Bike B5",   PlateNumber = "BN-005", Type = VehicleType.Bike, Status = VehicleStatus.Available,  Latitude = 10.8244m,  Longitude = 106.6332m, PricePerMinute = 0.10m, ProviderId = BikeNowId },

        // TaxiGo — 5 cars
        new() { Id = Guid.Parse("b4c5d6e7-f8a9-0123-bcde-234567890123"), Name = "TaxiGo Sedan",  PlateNumber = "TG-001", Type = VehicleType.Car, Status = VehicleStatus.Available,  Latitude = 10.8280m,  Longitude = 106.6360m, PricePerMinute = 0.50m, ProviderId = TaxiGoId },
        new() { Id = Guid.Parse("c5d6e7f8-a9b0-1234-cdef-345678901234"), Name = "TaxiGo SUV",    PlateNumber = "TG-002", Type = VehicleType.Car, Status = VehicleStatus.Available,  Latitude = 10.8291m,  Longitude = 106.6373m, PricePerMinute = 0.60m, ProviderId = TaxiGoId },
        new() { Id = Guid.Parse("d6e7f8a9-b0c1-2345-defa-456789012345"), Name = "TaxiGo Hatch", PlateNumber = "TG-003", Type = VehicleType.Car, Status = VehicleStatus.Available,  Latitude = 10.8302m,  Longitude = 106.6386m, PricePerMinute = 0.45m, ProviderId = TaxiGoId },
        new() { Id = Guid.Parse("e7f8a9b0-c1d2-3456-efab-567890123456"), Name = "TaxiGo Van",   PlateNumber = "TG-004", Type = VehicleType.Car, Status = VehicleStatus.InMaintenance, Latitude = 10.8313m,  Longitude = 106.6399m, PricePerMinute = 0.55m, ProviderId = TaxiGoId },
        new() { Id = Guid.Parse("f8a9b0c1-d2e3-4567-fabc-678901234567"), Name = "TaxiGo Lux",   PlateNumber = "TG-005", Type = VehicleType.Car, Status = VehicleStatus.Available,  Latitude = 10.8324m,  Longitude = 106.6412m, PricePerMinute = 0.80m, ProviderId = TaxiGoId },
    ];

    private static readonly Guid AdminUserId = Guid.Parse("00000000-0000-0000-0000-000000000001");
    private static readonly Guid MemberUserId = Guid.Parse("00000000-0000-0000-0000-000000000002");

    public static readonly User[] Users =
    [
        new()
        {
            Id = AdminUserId,
            Email = "admin@mobigate.io",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin!123"),
            Role = UserRole.Admin,
            CreatedAt = DateTime.UtcNow
        },
        new()
        {
            Id = MemberUserId,
            Email = "user@mobigate.io",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("User!123"),
            Role = UserRole.Member,
            CreatedAt = DateTime.UtcNow
        }
    ];

    public static async Task InitializeAsync(MobiGateDbContext db)
    {
        if (await db.Providers.AnyAsync()) return; // already seeded

        db.Providers.AddRange(Providers);
        db.Vehicles.AddRange(Vehicles);
        db.Users.AddRange(Users);
        await db.SaveChangesAsync();
    }
}
