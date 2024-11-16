using Microsoft.EntityFrameworkCore;
using MyCarAuctionAPI.Infrastructure.Data.Entities;
using MyCarAuctionAPI.Infrastructure.Data;
using MyCarAuctionAPI.Infrastructure.Repository;

namespace MyCarAuction.Api.Features.Bids.Repository
{
    public sealed class BidRepository(IDbContextFactory<ApiDbContext> dbContextFactory) : BaseRepository<BidEntity>(dbContextFactory), IBidRepository
    {
        public async Task<IEnumerable<BidEntity>> GetBidsByVehicleId(Guid vehicleId, CancellationToken cancellationToken)
        {
            await using var dbContext = await DbContextFactory.CreateDbContextAsync(cancellationToken);

            return await dbContext.Set<BidEntity>().Where(b => b.VehicleId == vehicleId).ToListAsync(cancellationToken);
        }
    }
}
