using Microsoft.EntityFrameworkCore;
using MyCarAuctionAPI.Infrastructure.Data;
using MyCarAuctionAPI.Infrastructure.Data.Entities;

namespace MyCarAuction.Api.Infrastructure.Data
{
    public sealed class Seed
    {
        public static async Task SeedData(IDbContextFactory<ApiDbContext> dbContextFactory)
        {
            await using var dbContext = await dbContextFactory.CreateDbContextAsync();

            await dbContext.Vehicles.AddRangeAsync(new VehicleEntity
                {
                    Id = Guid.NewGuid(),
                    Type = "SUV",
                    Manufacturer = "Volvo",
                    Model = "V40",
                    Year = 2024,
                    NumberOfDoors = 5,
                    NumberOfSeats = 7,
                    LoadCapacity = null,
                    StartingBid = 10
                }
            );

            await dbContext.SaveChangesAsync();
        }
    }
}
