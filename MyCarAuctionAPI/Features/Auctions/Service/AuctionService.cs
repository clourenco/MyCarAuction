using MyCarAuction.Api.Common.Exceptions;
using MyCarAuction.Api.Domain.Models;
using MyCarAuction.Api.Features.Auctions.Common.Exceptions;
using MyCarAuction.Api.Features.Auctions.Repository;
using MyCarAuction.Api.Features.Bids.Service;
using MyCarAuctionAPI.Domain.Models;
using MyCarAuctionAPI.Features.Vehicles.Interfaces.Services;
using MyCarAuctionAPI.Infrastructure.Data.Entities;

namespace MyCarAuction.Api.Features.Auctions.Service
{
    public sealed class AuctionService : IAuctionService
    {
        private readonly IAuctionRepository _auctionRepository;
        private readonly IVehicleService _vehicleService;
        private readonly IBidService _bidService;

        public AuctionService(IAuctionRepository auctionRepository, IVehicleService vehicleService, IBidService bidService)
        {
            _auctionRepository = auctionRepository ?? throw new ArgumentNullException(nameof(auctionRepository));
            _vehicleService = vehicleService ?? throw new ArgumentNullException(nameof(vehicleService));
            _bidService = bidService ?? throw new ArgumentNullException(nameof(bidService));
        }

        public async Task<Auction> GetAuction(Guid id, CancellationToken cancellationToken)
        {
            var existentAuction = await _auctionRepository.Get(id, cancellationToken);

            if (existentAuction == null)
                return MapToModel(existentAuction);
            else
                throw new AuctionNotFoundException($"Auction with id {id} was not found.");
        }

        public async Task<Auction> StartAuction(Auction auction, Guid vehicleId, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(auction);

            await IdIsInUse(auction.Id, cancellationToken);

            Vehicle existentVehicle = await GetVehicle(vehicleId, cancellationToken);

            await VehicleIsInAuction(vehicleId, cancellationToken);

            AuctionEntity auctionEntity = MapToEntity(auction);

            AuctionEntity? addedAuction = await CreateAuction(existentVehicle, auctionEntity, cancellationToken);

            return MapToModel(addedAuction);
        }

        private async Task<AuctionEntity?> CreateAuction(Vehicle existentVehicle, AuctionEntity auctionEntity, CancellationToken cancellationToken)
        {
            var addedAuction = await _auctionRepository.Create(auctionEntity, cancellationToken);

            if (addedAuction != null)
            {
                var bid = new Bid(
                    id: Guid.NewGuid(),
                    auctionId: auctionEntity.Id,
                    vehicleId: existentVehicle.Id,
                    bidderId: Guid.Empty, // means it's the system
                    amount: existentVehicle.StartingBid
                );

                var initialBid = await _bidService.PlaceBid(bid, cancellationToken);

                if (initialBid == null)
                {
                    var deleted = await _auctionRepository.Delete(addedAuction, cancellationToken);

                    if (deleted)
                        throw new InvalidOperationException("It was not possible to start the auction.");
                }
            }

            return addedAuction;
        }

        #region Private methods

        private async Task<Vehicle> GetVehicle(Guid vehicleId, CancellationToken cancellationToken)
        {
            return await _vehicleService.GetVehicle(vehicleId, cancellationToken) ?? throw new VehicleNotFoundException($"Vehicle with id {vehicleId} was found.");
        }

        private async Task IdIsInUse(Guid auctionId, CancellationToken cancellationToken)
        {
            var existentAuction = await _auctionRepository.Get(auctionId, cancellationToken);

            if (existentAuction != null)
                throw new KeyViolationException($"The provided id ({auctionId}) for the auction is already in use.");
        }

        private async Task VehicleIsInAuction(Guid vehicleId, CancellationToken cancellationToken)
        {
            var bidsByVehicle = await _bidService.GetBidsByVehicle(vehicleId, cancellationToken);

            if (bidsByVehicle?.Any() == true)
                throw new AuctionContainsVehicleException($"Vehicle with id {vehicleId} is already in an auction.");
        }

        public async Task<Auction> CloseAuction(Guid id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        private static Auction MapToModel(AuctionEntity entity)
            => new(
                id: entity.Id,
                isActive: entity.IsActive,
                description: entity.Description
            );

        private AuctionEntity MapToEntity(Auction model)
            => new()
            {
                Id = model.Id,
                IsActive = model.IsActive,
                Description = model.Description
            };

        #endregion
    }
}
