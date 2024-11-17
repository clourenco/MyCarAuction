using Microsoft.EntityFrameworkCore;
using MyCarAuction.Api.Features.Vehicles.Repository;
using MyCarAuction.Api.Infrastructure.Data;
using MyCarAuction.Api.Infrastructure.Data.Entities;
using MyCarAuction.Api.Infrastructure.Repository;

namespace MyCarAuctionAPI.Features.Vehicles.Repository;

internal sealed class VehicleRepository(IDbContextFactory<ApiDbContext> dbContextFactory) : BaseRepository<VehicleEntity>(dbContextFactory), IVehicleRepository
{
    public async Task<IEnumerable<VehicleEntity>> FindVehicle(
        string type,
        string manufacturer,
        string model,
        int? year,
        CancellationToken cancellationToken
    )
    {
        await using var dbContext = await DbContextFactory.CreateDbContextAsync(cancellationToken);

        return await dbContext.Set<VehicleEntity>().Where(v =>
            (type == null || v.Type.Equals(type, StringComparison.OrdinalIgnoreCase)) &&
            (manufacturer == null || v.Manufacturer.Equals(manufacturer, StringComparison.OrdinalIgnoreCase)) &&
            (model == null || v.Model.Equals(model, StringComparison.OrdinalIgnoreCase)) &&
            (year == null || v.Year == year)
        ).ToListAsync(cancellationToken);
    }
}
