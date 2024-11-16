using Microsoft.EntityFrameworkCore;
using MyCarAuction.Api.Features.Vehicles.Repository;
using MyCarAuctionAPI.Infrastructure.Data;
using MyCarAuctionAPI.Infrastructure.Data.Entities;
using MyCarAuctionAPI.Infrastructure.Repository;

namespace MyCarAuctionAPI.Features.Vehicles.Repository
{
    public sealed class VehicleRepository(IDbContextFactory<ApiDbContext> dbContextFactory) : BaseRepository<VehicleEntity>(dbContextFactory), IVehicleRepository
    {
        public async Task<IEnumerable<VehicleEntity>> FindVehicle(
            string? type,
            string? manufacturer,
            string? model,
            int? year,
            CancellationToken cancellationToken
        )
        {
            await using var dbContext = await DbContextFactory.CreateDbContextAsync(cancellationToken);

            return await dbContext.Set<VehicleEntity>().Where(v =>
                (type == null || string.IsNullOrWhiteSpace(type) || v.Type.Equals(type, StringComparison.OrdinalIgnoreCase)) &&
                (manufacturer == null || string.IsNullOrWhiteSpace(v.Manufacturer) || v.Manufacturer.Equals(manufacturer, StringComparison.OrdinalIgnoreCase)) &&
                (model == null || string.IsNullOrWhiteSpace(v.Model) || v.Model.Equals(model, StringComparison.OrdinalIgnoreCase)) &&
                (year == null || v.Year == year)
            ).ToListAsync(cancellationToken);
        }
    }
}
