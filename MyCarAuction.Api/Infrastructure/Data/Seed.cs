using Microsoft.EntityFrameworkCore;
using MyCarAuction.Api.Infrastructure.Data.Entities;

namespace MyCarAuction.Api.Infrastructure.Data;

internal sealed class Seed
{
    public static async Task SeedData(IDbContextFactory<ApiDbContext> dbContextFactory)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();

        await dbContext.Vehicles.AddRangeAsync(
            new VehicleEntity
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
            },
            new VehicleEntity
            {
                Id = Guid.NewGuid(),
                Type = "S",
                Manufacturer = "Mercedes",
                Model = "A180",
                Year = 2023,
                NumberOfDoors = 5,
                NumberOfSeats = 5,
                LoadCapacity = null,
                StartingBid = 20
            },
            new VehicleEntity
            {
                Id = Guid.NewGuid(),
                Type = "H",
                Manufacturer = "Audi",
                Model = "A5",
                Year = 2023,
                NumberOfDoors = 5,
                NumberOfSeats = 5,
                LoadCapacity = null,
                StartingBid = 30
            }
        );
        //await dbContext.Users.AddRangeAsync(
        //    new User
        //    (
        //        id: Guid.NewGuid(),
        //        name: "Bruce Wayne",
        //        email: "batman@mail.com"
        //    ),
        //    new User
        //    (
        //        id: Guid.NewGuid(),
        //        name: "Tony Stark",
        //        email: "ironman@mail.com"
        //    ),
        //    new User
        //    (
        //        id: Guid.NewGuid(),
        //        name: "Peter Parker",
        //        email: "spiderman@mail.com"
        //    )
        //);
        await dbContext.Users.AddRangeAsync(
            new UserEntity
            {
                Id = Guid.NewGuid(),
                Name = "Bruce Wayne",
                Email = "batman@mail.com"
            },
            new UserEntity
            {
                Id = Guid.NewGuid(),
                Name = "Tony Stark",
                Email = "ironman@mail.com"
            },
            new UserEntity
            {
                Id = Guid.NewGuid(),
                Name = "Peter Parker",
                Email = "spiderman@mail.com"
            }
        );

        await dbContext.SaveChangesAsync();
    }
}
