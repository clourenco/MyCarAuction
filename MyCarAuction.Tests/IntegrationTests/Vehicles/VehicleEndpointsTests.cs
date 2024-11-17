using MediatR;
using Moq;
using MyCarAuction.Api.Features.Vehicles.Commands.CreateVehicle;
using MyCarAuction.Tests.Factories;
using System.Net.Http.Json;
using System.Net;
using MyCarAuction.Api.Features.Users.Commands.CreateUser;
using FluentAssertions;
using MyCarAuction.Api.Features.Users.Queries.GetUser;
using MyCarAuction.Api.Features.Vehicles.Queries.GetVehicle;
using MyCarAuction.Api.Feature.User.Query.SearchUser;
using MyCarAuction.Api.Feature.Users.Query.SearchUser;
using MyCarAuction.Api.Features.Vehicles.Queries.SearchVehicle;

namespace MyCarAuction.Tests.IntegrationTests.Vehicles;

public class VehicleEndpointsTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly Mock<IMediator> _mediatorMock;

    public VehicleEndpointsTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
        _mediatorMock = factory.MediatorMock;
    }

    [Fact]
    public async Task AddVehicle_ShouldReturnCreated()
    {
        // Arrange
        var command = new CreateVehicleCommand(
            Type: "SUV",
            Manufacturer: "Toyota",
            Model: "RAV4",
            Year: 2022,
            NumberOfDoors: 5,
            NumberOfSeats: 7,
            LoadCapacity: null,
            StartingBid: 250
        );

        var expectedResult = new CreateVehicleResponse(
            id: Guid.NewGuid(),
            type: "SUV",
            manufacturer: "Toyota",
            model: "RAV4",
            year: 2022,
            numberOfDoors: 5,
            numberOfSeats: 7,
            loadCapacity: null,
            startingBid: 250
        );

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<CreateVehicleCommand>(), default))
            .ReturnsAsync(expectedResult);

        // Act
        var response = await _client.PostAsJsonAsync("/api/vehicles", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var location = response.Headers.Location;
        location.Should().NotBeNull();

        _mediatorMock.Verify(m => m.Send(It.IsAny<CreateVehicleCommand>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetVehicle_ShouldReturnOk()
    {
        // Arrange
        var vehicleId = Guid.NewGuid();
        var expectedResult = new GetVehicleResponse(
            id: Guid.NewGuid(),
            type: "SUV",
            manufacturer: "Toyota",
            model: "RAV4",
            year: 2022,
            numberOfDoors: 5,
            numberOfSeats: 7,
            loadCapacity: null,
            startingBid: 250
        );

        _mediatorMock
            .Setup(m => m.Send(It.Is<GetVehicleQuery>(q => q.Id == vehicleId), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResult);

        // Act
        var response = await _client.GetAsync($"/api/vehicles/{vehicleId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        _mediatorMock.Verify(m => m.Send(It.Is<GetVehicleQuery>(q => q.Id == vehicleId), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task SearchUsers_ShouldReturnOk()
    {
        // Arrange
        var queryResults = new List<SearchVehicleResponse>()
        {
            new(
                id: Guid.NewGuid(),
                type: "SUV",
                manufacturer: "Toyota",
                model: "RAV4",
                year: 2022,
                numberOfDoors: 5,
                numberOfSeats: 7,
                loadCapacity: null,
                startingBid: 250
            )
        };

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<SearchVehicleQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(queryResults);

        // Act
        var response = await _client.GetAsync("/api/vehicles?type=suv&manufacturer=toyota");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        _mediatorMock.Verify(m => m.Send(It.IsAny<SearchVehicleQuery>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}