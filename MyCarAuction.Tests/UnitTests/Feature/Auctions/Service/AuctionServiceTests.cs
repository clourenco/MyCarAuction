using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MyCarAuction.Api.Feature.Auctions.Service;
using MyCarAuction.Api.Features.Auctions.Repository;
using MyCarAuction.Api.Features.Vehicles.Interfaces.Services;
using MyCarAuction.Api.Features.Vehicles.Services;
using MyCarAuction.Api.Infrastructure.Data.Entities;
using MyCarAuction.Api.Domain.Models;
using MyCarAuction.Api.Feature.Auctions.Common.Exceptions;
using MyCarAuction.Api.Features.Auctions.Common.Exceptions;
using MyCarAuction.Api.Common.CustomException;
using Xunit;
using MyCarAuction.Api.Feature.Auctions.Repository;
using MyCarAuction.Api.Domain.Models.Enums;
using MyCarAuction.Api.Features.Users.Service;
using MyCarAuction.Api.Features.Users.Common.CustomException;

namespace MyCarAuction.Api.Tests.Features.Auctions.Service
{
    public class AuctionServiceTests
    {
        private readonly Mock<IAuctionRepository> _auctionRepositoryMock;
        private readonly Mock<IBidRepository> _bidRepositoryMock;
        private readonly Mock<IVehicleService> _vehicleServiceMock;
        private readonly Mock<IUserService> _userServiceMock;
        private readonly AuctionService _auctionService;

        public AuctionServiceTests()
        {
            _auctionRepositoryMock = new Mock<IAuctionRepository>();
            _bidRepositoryMock = new Mock<IBidRepository>();
            _vehicleServiceMock = new Mock<IVehicleService>();
            _userServiceMock = new Mock<IUserService>();
            _auctionService = new AuctionService(
                _auctionRepositoryMock.Object,
                _bidRepositoryMock.Object,
                _vehicleServiceMock.Object,
                _userServiceMock.Object
            );
        }

        [Fact]
        public async Task StartAuction_ShouldReturnAuction_WhenAuctionStartsSuccessfully()
        {
            // Arrange
            var auctionId = Guid.NewGuid();
            var vehicleId = Guid.NewGuid();

            var auctionEntity = new AuctionEntity
            {
                Id = auctionId,
                IsActive = true,
                Description = "Auction 1"
            };

            var auction = new Auction(
                id: auctionEntity.Id,
                isActive: true,
                description: auctionEntity.Description
            );

            var vehicle = new Vehicle(
                id: vehicleId,
                type: VehicleType.H,
                manufacturer: "Volvo",
                model: "V40",
                year: 2024,
                numberOfDoors: 5,
                numberOfSeats: 5,
                loadCapacity: null,
                startingBid: 100
            );

            _auctionRepositoryMock
                .Setup(repo => repo.Get(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((AuctionEntity)null);

            _vehicleServiceMock
                .Setup(v => v.GetVehicle(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(vehicle);

            _bidRepositoryMock.Setup(b => b.GetBidsByAuctionIdVehicleId(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<BidEntity>());

            _auctionRepositoryMock.Setup(repo => repo.Create(It.IsAny<AuctionEntity>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(auctionEntity);

            // Act
            var result = await _auctionService.StartAuction(auction, vehicleId, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(auctionId, result.Id);
            Assert.True(result.IsActive);
            Assert.Equal("Auction 1", result.Description);
        }

        [Fact]
        public async Task StartAuction_ShouldThrowVehicleNotFoundException_WhenVehicleNotFound()
        {
            // Arrange
            var auction = new Auction(
                id: Guid.NewGuid(),
                isActive: true,
                description: "Auction 1"
            );

            var vehicleId = Guid.NewGuid();

            _vehicleServiceMock
                .Setup(v => v.GetVehicle(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Vehicle)null); // Vehicle not found

            // Act & Assert
            var exception = await Assert.ThrowsAsync<VehicleNotFoundException>(() =>
                _auctionService.StartAuction(auction, vehicleId, CancellationToken.None));

            Assert.Equal($"The vehicle with id {vehicleId} was found.", exception.Message);
        }

        [Fact]
        public async Task CloseAuction_ShouldReturnAuction_WhenAuctionIsClosedSuccessfully()
        {
            // Arrange
            var auctionId = Guid.NewGuid();
            var auctionEntity = new AuctionEntity { Id = auctionId, IsActive = true, Description = "Auction 1" };

            _auctionRepositoryMock
                .Setup(repo => repo.Get(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(auctionEntity);

            _auctionRepositoryMock
                .Setup(repo => repo.Update(It.IsAny<AuctionEntity>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(auctionEntity); // Return updated auction

            // Act
            var result = await _auctionService.CloseAuction(auctionId, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.IsActive);
        }

        [Fact]
        public async Task CloseAuction_ShouldThrowAuctionNotFoundException_WhenAuctionDoesNotExist()
        {
            // Arrange
            var auctionId = Guid.NewGuid();

            _auctionRepositoryMock
                .Setup(repo => repo.Get(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((AuctionEntity)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<AuctionNotFoundException>(() =>
                _auctionService.CloseAuction(auctionId, CancellationToken.None));

            Assert.Equal($"The auction with id {auctionId} was not found.", exception.Message);
        }

        [Fact]
        public async Task GetAuction_ShouldReturnAuction_WhenAuctionExists()
        {
            // Arrange
            var auctionId = Guid.NewGuid();
            var auctionEntity = new AuctionEntity { Id = auctionId, IsActive = true, Description = "Auction 1" };

            _auctionRepositoryMock
                .Setup(repo => repo.Get(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(auctionEntity);

            // Act
            var result = await _auctionService.GetAuction(auctionId, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(auctionId, result.Id);
            Assert.True(result.IsActive);
            Assert.Equal("Auction 1", result.Description);
        }

        [Fact]
        public async Task GetAuction_ShouldThrowAuctionNotFoundException_WhenAuctionDoesNotExist()
        {
            // Arrange
            var auctionId = Guid.NewGuid();

            _auctionRepositoryMock
                .Setup(repo => repo.Get(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((AuctionEntity)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<AuctionNotFoundException>(() =>
                _auctionService.GetAuction(auctionId, CancellationToken.None));

            Assert.Equal($"The auction with id {auctionId} was not found.", exception.Message);
        }

        [Fact]
        public async Task PlaceBid_ShouldReturnBid_WhenBidIsPlacedSuccessfully()
        {
            // Arrange
            var existentAuctionId = Guid.NewGuid();
            var existentVehicleId = Guid.NewGuid();
            var existentBidderId = Guid.NewGuid();

            var existentBids = new List<BidEntity>()
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    AuctionId = existentAuctionId,
                    VehicleId = existentVehicleId,
                    BidderId = existentBidderId,
                    Amount = 50,
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    AuctionId = existentAuctionId,
                    VehicleId = existentVehicleId,
                    BidderId = existentBidderId,
                    Amount = 100,
                }
            };

            var bidder = new User(
                id: existentBidderId,
                name: "John Doe",
                email: "john.doe@mail.com"
            );

            var bid = new Bid(
                id: Guid.NewGuid(),
                auctionId: existentAuctionId,
                vehicleId: existentVehicleId,
                bidderId: existentBidderId,
                amount: 150
            );

            var auctionEntity = new AuctionEntity { Id = bid.AuctionId, IsActive = true, Description = "Auction 1" };
            var bidEntity = new BidEntity { Id = bid.Id, AuctionId = bid.AuctionId, VehicleId = bid.VehicleId, Amount = bid.Amount };

            _auctionRepositoryMock
                .Setup(repo => repo.Get(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(auctionEntity);

            _bidRepositoryMock
                .Setup(b => b.GetBidsByAuctionIdVehicleId(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(existentBids);

            _userServiceMock
                .Setup(u => u.GetUser(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(bidder);

            _bidRepositoryMock
                .Setup(b => b.GetMaxAmountBidByAuctionIdVehicleId(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(100);

            _bidRepositoryMock
                .Setup(b => b.Create(It.IsAny<BidEntity>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(bidEntity);

            // Act
            var result = await _auctionService.PlaceBid(bid, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(bid.Id, result.Id);
            Assert.Equal(bid.Amount, result.Amount);
        }

        [Fact]
        public async Task PlaceBid_ShouldThrowAuctionNotFoundException_WhenAuctionDoesNotExist()
        {
            // Arrange
            var bid = new Bid(
                id: Guid.NewGuid(),
                auctionId: Guid.NewGuid(),
                vehicleId: Guid.NewGuid(),
                bidderId: Guid.NewGuid(),
                amount: 150
            );

            _auctionRepositoryMock
                .Setup(repo => repo.Get(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((AuctionEntity)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<AuctionNotFoundException>(() =>
                _auctionService.PlaceBid(bid, CancellationToken.None));
            Assert.Equal($"The auction with id {bid.AuctionId} was not found.", exception.Message);
        }

        [Fact]
        public async Task PlaceBid_ShouldThrowAuctionNotActiveException_WhenAuctionIsNotActive()
        {
            // Arrange
            var bid = new Bid(
                id: Guid.NewGuid(),
                auctionId: Guid.NewGuid(),
                vehicleId: Guid.NewGuid(),
                bidderId: Guid.NewGuid(),
                amount: 150
            );

            var auctionEntity = new AuctionEntity { Id = bid.AuctionId, IsActive = false, Description = "Auction 1" };

            _auctionRepositoryMock
                .Setup(repo => repo.Get(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(auctionEntity);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<AuctionNotActiveException>(() =>
                _auctionService.PlaceBid(bid, CancellationToken.None));

            Assert.Equal($"The auction with id {bid.AuctionId} is not active.", exception.Message);
        }

        [Fact]
        public async Task PlaceBid_ShouldThrowVehicleNotInAuctionException_WhenVehicleIsNotInAuction()
        {
            // Arrange
            var bid = new Bid(
                id: Guid.NewGuid(),
                auctionId: Guid.NewGuid(),
                vehicleId: Guid.NewGuid(),
                bidderId: Guid.NewGuid(),
                amount: 500
            );

            // Mock: Auction exists and is active
            _auctionRepositoryMock.Setup(repo => repo.Get(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new AuctionEntity { Id = bid.AuctionId, IsActive = true });

            // Mock: Vehicle is not in the auction
            _bidRepositoryMock.Setup(repo => repo.GetBidsByAuctionIdVehicleId(bid.AuctionId, bid.VehicleId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<BidEntity>()); // No bids for this vehicle in the auction

            // Act & Assert
            var exception = await Assert.ThrowsAsync<VehicleNotInAuctionException>(() =>
                _auctionService.PlaceBid(bid, CancellationToken.None)
            );

            Assert.Equal($"The vehicle with id {bid.VehicleId} is not in the auction with id {bid.AuctionId}.", exception.Message);
        }

        [Fact]
        public async Task PlaceBid_ShouldThrowUserNotFoundException_WhenBidderIsNotFound()
        {
            // Arrange
            var bid = new Bid(
                id: Guid.NewGuid(),
                auctionId: Guid.NewGuid(),
                vehicleId: Guid.NewGuid(),
                bidderId: Guid.NewGuid(),
                amount: 500
            );

            // Mock: Auction exists and is active
            _auctionRepositoryMock.Setup(repo => repo.Get(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new AuctionEntity { Id = bid.AuctionId, IsActive = true });

            // Mock: Vehicle is in the auction
            _bidRepositoryMock.Setup(repo => repo.GetBidsByAuctionIdVehicleId(bid.AuctionId, bid.VehicleId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<BidEntity> { new BidEntity { Id = Guid.NewGuid() } });

            // Mock: Bidder does not exist
            _userServiceMock.Setup(repo => repo.GetUser(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((User)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<UserNotFoundException>(() =>
                _auctionService.PlaceBid(bid, CancellationToken.None)
            );

            Assert.Equal($"The bidder with id {bid.BidderId} was not found.", exception.Message);
        }

    }
}
