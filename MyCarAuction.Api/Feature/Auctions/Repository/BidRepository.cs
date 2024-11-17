using Microsoft.EntityFrameworkCore;
using MyCarAuction.Api.Infrastructure.Data.Entities;
using MyCarAuction.Api.Infrastructure.Data;
using MyCarAuction.Api.Infrastructure.Repository;

namespace MyCarAuction.Api.Feature.Auctions.Repository
{
    internal sealed class BidRepository(IDbContextFactory<ApiDbContext> dbContextFactory) : BaseRepository<BidEntity>(dbContextFactory), IBidRepository
    {
        public async Task<IEnumerable<BidEntity>> GetBidsByAuctionIdVehicleId(Guid auctionId, Guid vehicleId, CancellationToken cancellationToken)
        {
            await using var dbContext = await DbContextFactory.CreateDbContextAsync(cancellationToken);

            return await dbContext.Set<BidEntity>().Where(b => b.AuctionId == auctionId && b.VehicleId == vehicleId).ToListAsync(cancellationToken);
        }

        public async Task<decimal> GetMaxAmountBidByAuctionIdVehicleId(Guid auctionId, Guid vehicleId, CancellationToken cancellationToken)
        {
            await using var dbContext = await DbContextFactory.CreateDbContextAsync(cancellationToken);

            return await dbContext.Set<BidEntity>().Where(b => b.AuctionId == auctionId && b.VehicleId == vehicleId).MaxAsync(b => b.Amount);
        }
    }
}
