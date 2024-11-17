using Moq;
using MyCarAuction.Api.Common.CustomException;
using MyCarAuction.Api.Domain.Models;
using MyCarAuction.Api.Domain.Models.Enums;
using MyCarAuction.Api.Features.Vehicles.Repository;
using MyCarAuction.Api.Features.Vehicles.Services;
using MyCarAuction.Api.Infrastructure.Data.Entities;

namespace MyCarAuction.Tests.UnitTests.Feature.Vehicles.Service;

public class UserServiceTests
{
    private readonly Mock<IVehicleRepository> _mockVehicleRepository;
    private readonly VehicleService _userService;

    public UserServiceTests()
    {
        _mockVehicleRepository = new Mock<IVehicleRepository>();
        _userService = new VehicleService(_mockVehicleRepository.Object);
    }

    [Fact]
    public async Task GetVehicle_ShouldReturnVehicle_WhenVehicleExists()
    {
        // Arrange
        var vehicleId = Guid.NewGuid();
        var vehicleEntity = new VehicleEntity
        {
            Id = vehicleId,
            Type = "SUV",
            Manufacturer = "Toyota",
            Model = "Corolla",
            Year = 2020,
            NumberOfDoors = 4,
            NumberOfSeats = 5,
            LoadCapacity = null,
            StartingBid = 10000
        };

        _mockVehicleRepository
            .Setup(repo => repo.Get(vehicleId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(vehicleEntity);

        // Act
        var result = await _userService.GetVehicle(vehicleId, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(vehicleId, result.Id);
        Assert.Equal(VehicleType.SUV, result.Type);
        Assert.Equal("Toyota", result.Manufacturer);

        _mockVehicleRepository.Verify(repo => repo.Get(vehicleId, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetVehicle_ShouldThrowVehicleNotFoundException_WhenVehicleDoesNotExist()
    {
        // Arrange
        var vehicleId = Guid.NewGuid();

        _mockVehicleRepository
            .Setup(repo => repo.Get(vehicleId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((VehicleEntity)null);

        // Act & Assert
        await Assert.ThrowsAsync<VehicleNotFoundException>(
            () => _userService.GetVehicle(vehicleId, CancellationToken.None));

        _mockVehicleRepository.Verify(repo => repo.Get(vehicleId, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task AddVehicle_ShouldAddVehicle_WhenVehicleIsValidAndDoesNotExist()
    {
        // Arrange
        var vehicle = new Vehicle(
            Guid.NewGuid(),
            VehicleType.T,
            "Ford",
            "F-150",
            2022,
            2,
            3,
            1000,
            20000);

        _mockVehicleRepository
            .Setup(repo => repo.Get(vehicle.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((VehicleEntity)null);

        _mockVehicleRepository
            .Setup(repo => repo.Create(It.IsAny<VehicleEntity>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new VehicleEntity
            {
                Id = vehicle.Id,
                Type = vehicle.Type.ToString(),
                Manufacturer = vehicle.Manufacturer,
                Model = vehicle.Model,
                Year = vehicle.Year,
                NumberOfDoors = vehicle.NumberOfDoors,
                NumberOfSeats = vehicle.NumberOfSeats,
                LoadCapacity = vehicle.LoadCapacity,
                StartingBid = vehicle.StartingBid
            });

        // Act
        var result = await _userService.AddVehicle(vehicle, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(vehicle.Id, result.Id);
        Assert.Equal(vehicle.Type, result.Type);

        _mockVehicleRepository.Verify(repo => repo.Get(vehicle.Id, It.IsAny<CancellationToken>()), Times.Once);
        _mockVehicleRepository.Verify(repo => repo.Create(It.IsAny<VehicleEntity>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task AddVehicle_ShouldThrowKeyViolationException_WhenVehicleAlreadyExists()
    {
        // Arrange
        var vehicle = new Vehicle(
            Guid.NewGuid(),
            VehicleType.S,
            "Honda",
            "Civic",
            2021,
            4,
            5,
            null,
            15000);

        var existingEntity = new VehicleEntity
        {
            Id = vehicle.Id,
            Type = vehicle.Type.ToString(),
            Manufacturer = vehicle.Manufacturer,
            Model = vehicle.Model,
            Year = vehicle.Year,
            NumberOfDoors = vehicle.NumberOfDoors,
            NumberOfSeats = vehicle.NumberOfSeats,
            LoadCapacity = vehicle.LoadCapacity,
            StartingBid = vehicle.StartingBid
        };

        _mockVehicleRepository
            .Setup(repo => repo.Get(vehicle.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingEntity);

        // Act & Assert
        await Assert.ThrowsAsync<KeyViolationException>(
            () => _userService.AddVehicle(vehicle, CancellationToken.None));

        _mockVehicleRepository.Verify(repo => repo.Get(vehicle.Id, It.IsAny<CancellationToken>()), Times.Once);
        _mockVehicleRepository.Verify(repo => repo.Create(It.IsAny<VehicleEntity>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task SearchVehicle_ShouldReturnVehicles_WhenMatchingVehiclesExist()
    {
        // Arrange
        var searchType = "H";
        var searchManufacturer = "Toyota";
        var searchModel = "Corolla";
        int? searchYear = 2020;

        var vehicleEntities = new List<VehicleEntity>
        {
            new() {
                Id = Guid.NewGuid(),
                Type = "H",
                Manufacturer = "Toyota",
                Model = "Corolla",
                Year = 2020,
                NumberOfDoors = 4,
                NumberOfSeats = 5,
                LoadCapacity = null,
                StartingBid = 15000
            },
            new() {
                Id = Guid.NewGuid(),
                Type = "T",
                Manufacturer = "Ford",
                Model = "F-150",
                Year = 2020,
                NumberOfDoors = 4,
                NumberOfSeats = 2,
                LoadCapacity = 2000,
                StartingBid = 25000
            }
        };

        _mockVehicleRepository
            .Setup(repo => repo.FindVehicle(It.Is<string>(type => type == searchType), searchManufacturer, searchModel, searchYear, It.IsAny<CancellationToken>()))
            .ReturnsAsync(vehicleEntities.Where(v => v.Type == searchType).ToList());

        // Act
        var result = await _userService.SearchVehicle(searchType, searchManufacturer, searchModel, searchYear, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Single(result);
        Assert.Equal("Toyota", result.First().Manufacturer);

        _mockVehicleRepository.Verify(repo => repo.FindVehicle(It.Is<string>(type => type == searchType), searchManufacturer, searchModel, searchYear, It.IsAny<CancellationToken>()), Times.Once);
    }
}
