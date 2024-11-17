using Microsoft.EntityFrameworkCore;
using MyCarAuction.Api.Infrastructure.Data;
using MyCarAuction.Api.Infrastructure.Data.Entities;
using MyCarAuction.Api.Infrastructure.Repository;

namespace MyCarAuction.Api.Features.Auctions.Repository;

internal sealed class AuctionRepository(IDbContextFactory<ApiDbContext> dbContextFactory) : BaseRepository<AuctionEntity>(dbContextFactory), IAuctionRepository
{
}
