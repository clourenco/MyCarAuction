using MyCarAuction.Api.Common;
using MyCarAuction.Api.Domain.Models;
using MyCarAuction.Api.Features.Bids.Repository;
using MyCarAuctionAPI.Domain.Models;
using MyCarAuctionAPI.Domain.Models.Enums;
using MyCarAuctionAPI.Infrastructure.Data.Entities;

namespace MyCarAuction.Api.Features.Bids.Service
{
    public sealed class BidService : IBidService
    {
        private readonly IBidRepository _bidRepository;

        public BidService(IBidRepository bidRepository)
        {
            _bidRepository = bidRepository ?? throw new ArgumentNullException(nameof(bidRepository));
        }

        public async Task<Bid> PlaceBid(Bid bid, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(bid);

            BidEntity bidEntity = MapToEntity(bid);

            var addedBid = await _bidRepository.Create(bidEntity, cancellationToken);

            return MapToModel(addedBid);
        }

        public async Task<IEnumerable<Bid>> GetBidsByVehicle(Guid vehicleId, CancellationToken cancellationToken)
        {
            var bids = await _bidRepository.GetBidsByVehicleId(vehicleId, cancellationToken);
            return bids.Select(e => MapToModel(e));
        }

        #region Private methods

        private static Bid MapToModel(BidEntity entity)
            => new(
                id: entity.Id,
                auctionId: entity.AuctionId,
                vehicleId: entity.VehicleId,
                bidderId: entity.BidderId,
                amount: entity.Amount
            );

        private static BidEntity MapToEntity(Bid model)
        {
            return new()
            {
                Id = model.Id,
                AuctionId = model.AuctionId,
                VehicleId = model.VehicleId,
                BidderId = model.BidderId,
                Amount = model.Amount
            };
        }

        #endregion
    }
}
