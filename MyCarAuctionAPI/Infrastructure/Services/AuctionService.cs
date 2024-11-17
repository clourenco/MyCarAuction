using MyCarAuction.Api.Common.Exceptions;
using MyCarAuction.Api.Domain.Models;
using MyCarAuction.Api.Features.Auctions.Common.Exceptions;
using MyCarAuction.Api.Features.Auctions.Repository;
using MyCarAuction.Api.Features.Bids.Common.Exceptions;
using MyCarAuction.Api.Features.Bids.Repository;
using MyCarAuction.Api.Features.Vehicles.Repository;
using MyCarAuctionAPI.Domain.Models;
using MyCarAuctionAPI.Infrastructure.Data.Entities;

namespace MyCarAuction.Api.Infrastructure.Services
{
    public sealed class AuctionService : IAuctionService
    {
        private readonly IAuctionRepository _auctionRepository;
        private readonly IBidRepository _bidRepository;
        private readonly IVehicleRepository _vehicleRepository;

        public AuctionService(IAuctionRepository auctionRepository, IBidRepository bidRepository, IVehicleRepository vehicleRepository)
        {
            _auctionRepository = auctionRepository ?? throw new ArgumentNullException(nameof(auctionRepository));
            _bidRepository = bidRepository ?? throw new ArgumentNullException(nameof(bidRepository));
            _vehicleRepository = vehicleRepository ?? throw new ArgumentNullException(nameof(vehicleRepository));
        }

        public async Task<Auction> StartAuction(Auction auction, Guid vehicleId, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(auction);

            await IdIsInUse(auction.Id, cancellationToken);

            VehicleEntity existentVehicle = await _vehicleRepository.Get(vehicleId, cancellationToken) ?? throw new VehicleNotFoundException($"The vehicle with id {vehicleId} was found.");

            if (await IsVehicleInAuction(auction.Id, vehicleId, cancellationToken))
                throw new AuctionContainsVehicleException($"The vehicle with id {vehicleId} is already in an auction.");

            AuctionEntity auctionEntity = MapToAuctionEntity(auction);

            AuctionEntity? addedAuction = await CreateAuction(existentVehicle, auctionEntity, cancellationToken);

            return MapToAuctionModel(addedAuction);
        }

        public async Task<Auction> CloseAuction(Guid id, CancellationToken cancellationToken)
        {
            var existentAuction = await _auctionRepository.Get(id, cancellationToken) ?? throw new AuctionNotFoundException($"The auction with id {id} was not found.");
            existentAuction.IsActive = false;

            var updatedAuction = await _auctionRepository.Update(existentAuction, cancellationToken);
            return MapToAuctionModel(updatedAuction);
        }

        public async Task<Auction> GetAuction(Guid id, CancellationToken cancellationToken)
        {
            var existentAuction = await _auctionRepository.Get(id, cancellationToken);

            if (existentAuction != null)
                return MapToAuctionModel(existentAuction);
            else
                throw new AuctionNotFoundException($"The auction with id {id} was not found.");
        }

        public async Task<Bid> PlaceBid(Bid bid, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(bid);

            var auction = await _auctionRepository.Get(bid.AuctionId, cancellationToken) ?? throw new AuctionNotFoundException($"The auction with id {bid.AuctionId} was not found.");

            if (!auction.IsActive)
                throw new AuctionNotActiveException($"The auction with id {auction.Id} is not active.");

            if (!await IsVehicleInAuction(bid.AuctionId, bid.VehicleId, cancellationToken))
                throw new VehicleNotInAuctionException($"The vehicle with id {bid.VehicleId} is not in the auction with id {bid.AuctionId}.");

            await IsCurrentBidHigherThanHighestBid(bid, cancellationToken);

            BidEntity bidEntity = MapToBidEntity(bid);

            var addedBid = await _bidRepository.Create(bidEntity, cancellationToken);

            return MapToBidModel(addedBid);
        }

        #region Private methods

        private async Task IsCurrentBidHigherThanHighestBid(Bid bid, CancellationToken cancellationToken)
        {
            var currentBidAmount = bid.Amount;
            var highestBidAmount = await _bidRepository.GetMaxAmountBidByAuctionIdVehicleId(bid.AuctionId, bid.VehicleId, cancellationToken);

            if (currentBidAmount < highestBidAmount)
                throw new BidAmountSmallerThanHighestBidAmountException($"The current bid amount ({currentBidAmount}) is smaller than the highest bid amount ({highestBidAmount})");
        }

        private async Task<AuctionEntity?> CreateAuction(VehicleEntity existentVehicle, AuctionEntity auctionEntity, CancellationToken cancellationToken)
        {
            var addedAuction = await _auctionRepository.Create(auctionEntity, cancellationToken);

            if (addedAuction != null)
            {
                var bid = new Bid(
                    id: Guid.NewGuid(),
                    auctionId: auctionEntity.Id,
                    vehicleId: existentVehicle.Id,
                    bidderId: Guid.Empty,
                    amount: existentVehicle.StartingBid
                );

                var initialBid = await PlaceBid(bid, cancellationToken);

                if (initialBid == null)
                {
                    var deleted = await _auctionRepository.Delete(addedAuction, cancellationToken);

                    if (deleted)
                        throw new InvalidOperationException("It was not possible to start the auction.");
                }
            }

            return addedAuction;
        }

        private async Task<bool> IsVehicleInAuction(Guid auctionId, Guid vehicleId, CancellationToken cancellationToken)
        {
            var bidsByVehicle = await _bidRepository.GetBidsByAuctionIdVehicleId(auctionId, vehicleId, cancellationToken);
            return bidsByVehicle.Any();
            //if (bidsByVehicle?.Any() == true)
            //    throw new AuctionContainsVehicleException($"The vehicle with id {vehicleId} is already in an auction.");
        }

        private async Task IdIsInUse(Guid auctionId, CancellationToken cancellationToken)
        {
            var existentAuction = await _auctionRepository.Get(auctionId, cancellationToken);

            if (existentAuction != null)
                throw new KeyViolationException($"The provided id ({auctionId}) for the auction is already in use.");
        }

        private static Bid MapToBidModel(BidEntity entity)
            => new(
                id: entity.Id,
                auctionId: entity.AuctionId,
                vehicleId: entity.VehicleId,
                bidderId: entity.BidderId,
                amount: entity.Amount
            );

        private static BidEntity MapToBidEntity(Bid model)
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

        private static Auction MapToAuctionModel(AuctionEntity entity)
            => new(
                id: entity.Id,
                isActive: entity.IsActive,
                description: entity.Description
            );

        private AuctionEntity MapToAuctionEntity(Auction model)
            => new()
            {
                Id = model.Id,
                IsActive = model.IsActive,
                Description = model.Description
            };

        #endregion
    }
}
