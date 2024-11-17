using MyCarAuction.Api.Infrastructure.Data.Entities;
using MyCarAuction.Api.Infrastructure.Repository;

namespace MyCarAuction.Api.Features.Auctions.Repository;

internal interface IAuctionRepository : IBaseRepository<AuctionEntity>
{
}
