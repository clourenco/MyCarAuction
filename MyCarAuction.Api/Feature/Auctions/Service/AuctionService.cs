using MyCarAuction.Api.Common.CustomException;
using MyCarAuction.Api.Domain.Models;
using MyCarAuction.Api.Feature.Auctions.Common.Exceptions;
using MyCarAuction.Api.Feature.Auctions.Repository;
using MyCarAuction.Api.Features.Auctions.Common.Exceptions;
using MyCarAuction.Api.Features.Auctions.Repository;
using MyCarAuction.Api.Features.Users.Common.CustomException;
using MyCarAuction.Api.Features.Users.Service;
using MyCarAuction.Api.Features.Vehicles.Interfaces.Services;
using MyCarAuction.Api.Features.Vehicles.Repository;
using MyCarAuction.Api.Features.Vehicles.Services;
using MyCarAuction.Api.Infrastructure.Data.Entities;
using System.Security.Principal;

namespace MyCarAuction.Api.Feature.Auctions.Service;

internal sealed class AuctionService : IAuctionService
{
    private readonly IAuctionRepository _auctionRepository;
    private readonly IBidRepository _bidRepository;
    private readonly IVehicleService _vehicleService;
    private readonly IUserService _userService;

    public AuctionService(
        IAuctionRepository auctionRepository,
        IBidRepository bidRepository,
        IVehicleService vehicleService,
        IUserService userService
    )
    {
        _auctionRepository = auctionRepository ?? throw new ArgumentNullException(nameof(auctionRepository));
        _bidRepository = bidRepository ?? throw new ArgumentNullException(nameof(bidRepository));
        _vehicleService = vehicleService ?? throw new ArgumentNullException(nameof(vehicleService));
        _userService = userService ?? throw new ArgumentNullException(nameof(userService)); ;
    }

    public async Task<Auction> StartAuction(Auction auction, Guid vehicleId, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(auction);
        await IdIsInUse(auction.Id, cancellationToken);

        Vehicle existentVehicle = await _vehicleService.GetVehicle(vehicleId, cancellationToken) ?? throw new VehicleNotFoundException($"The vehicle with id {vehicleId} was found.");

        if (await WasVehicleSold(auction, vehicleId, cancellationToken))
            throw new VehicleSoldException($"The vehicle with the id {vehicleId} was sold.");

        if (auction.IsActive && await IsVehicleInAuction(auction.Id, vehicleId, cancellationToken))
            throw new AuctionContainsVehicleException($"The vehicle with id {vehicleId} is already in an active auction.");

        AuctionEntity auctionEntity = MapToAuctionEntity(auction);

        AuctionEntity? addedAuction = await CreateAuction(auctionEntity, existentVehicle, cancellationToken);

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

        var auction = await _auctionRepository.Get(bid.AuctionId, cancellationToken) ??
            throw new AuctionNotFoundException($"The auction with id {bid.AuctionId} was not found.");

        if (!auction.IsActive)
            throw new AuctionNotActiveException($"The auction with id {auction.Id} is not active.");

        if (!await IsVehicleInAuction(bid.AuctionId, bid.VehicleId, cancellationToken))
            throw new VehicleNotInAuctionException($"The vehicle with id {bid.VehicleId} is not in the auction with id {bid.AuctionId}.");

        if (await _userService.GetUser(bid.BidderId, cancellationToken) == null)
            throw new UserNotFoundException($"The bidder with id {bid.BidderId} was not found.");

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

    private async Task<AuctionEntity?> CreateAuction(AuctionEntity auctionEntity, Vehicle vehicle, CancellationToken cancellationToken)
    {
        var addedAuction = await _auctionRepository.Create(auctionEntity, cancellationToken);

        if (addedAuction != null)
        {
            var bidEntity = new BidEntity
            {
                Id = Guid.NewGuid(),
                AuctionId = addedAuction.Id,
                VehicleId = vehicle.Id,
                BidderId = Guid.Empty,
                Amount = vehicle.StartingBid
            };

            var initialBid = await _bidRepository.Create(bidEntity, cancellationToken);

            if (initialBid == null)
            {
                var deleted = await _auctionRepository.Delete(addedAuction, cancellationToken);

                if (deleted)
                    throw new InvalidOperationException("It was not possible to start the auction.");
            }
        }

        return addedAuction;
    }

    private async Task<bool> WasVehicleSold(Auction auction, Guid vehicleId, CancellationToken cancellationToken)
    {
        var bidsByVehicle = await _bidRepository.GetBidsByAuctionIdVehicleId(auction.Id, vehicleId, cancellationToken);
        return !auction.IsActive && bidsByVehicle.Any() && bidsByVehicle.Count() > 1;
    }

    private async Task<bool> IsVehicleInAuction(Guid auctionId, Guid vehicleId, CancellationToken cancellationToken)
    {
        var bidsByVehicle = await _bidRepository.GetBidsByAuctionIdVehicleId(auctionId, vehicleId, cancellationToken);
        return bidsByVehicle.Any();
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
