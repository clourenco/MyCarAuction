using FluentAssertions;
using MediatR;
using Moq;
using MyCarAuction.Api.Features.Auctions.Commands.StartAuction;
using MyCarAuction.Tests.Factories;
using System.Net.Http.Json;
using System.Net;
using MyCarAuction.Api.Features.Auctions.Commands.CloseAuction;
using MyCarAuction.Api.Features.Auctions.Queries.GetAuction;
using MyCarAuction.Api.Feature.Auctions.Command.PlaceBid;

namespace MyCarAuction.Tests.IntegrationTests.Auctions;

public class AuctionEndpointsTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly Mock<IMediator> _mediatorMock;

    public AuctionEndpointsTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
        _mediatorMock = factory.MediatorMock;
    }

    [Fact]
    public async Task StartAuction_ShouldReturnCreated()
    {
        // Arrange
        var command = new StartAuctionCommand(Guid.NewGuid(), "Test");
        var expectedResult = new StartAuctionResponse(Guid.NewGuid(), true, "Test");

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<StartAuctionCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResult);

        // Act
        var response = await _client.PostAsJsonAsync("/api/auctions", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        _mediatorMock.Verify(m => m.Send(It.IsAny<StartAuctionCommand>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task CloseAuction_ShouldReturnOk()
    {
        // Arrange
        var auctionId = Guid.NewGuid();
        var command = new CloseAuctionCommand(auctionId);
        var expectedResult = new CloseAuctionResponse(auctionId, false, "Test");

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<CloseAuctionCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResult);

        // Act
        var response = await _client.PutAsJsonAsync($"/api/auction/{auctionId}", new { });

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        _mediatorMock.Verify(m => m.Send(It.IsAny<CloseAuctionCommand>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetAuction_ShouldReturnOk()
    {
        // Arrange
        var auctionId = Guid.NewGuid();
        var expectedResult = new GetAuctionResponse(auctionId, true, "Test");

        _mediatorMock
            .Setup(m => m.Send(It.Is<GetAuctionQuery>(q => q.Id == auctionId), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResult);

        // Act
        var response = await _client.GetAsync($"/api/auctions/{auctionId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        _mediatorMock.Verify(m => m.Send(It.Is<GetAuctionQuery>(q => q.Id == auctionId), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task PlaceBid_ShouldReturnCreated()
    {
        // Arrange
        var auctionId = new Guid();
        var vehicleId = Guid.NewGuid();
        var bidderId = Guid.NewGuid();
        var amount = 150.05m;

        var command = new PlaceBidCommand(auctionId, vehicleId, bidderId, amount);
        var expectedResult = new PlaceBidResponse(Guid.NewGuid(), auctionId, vehicleId, bidderId, amount);

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<PlaceBidCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResult);

        // Act
        var response = await _client.PostAsJsonAsync("/api/auction/bid", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        _mediatorMock.Verify(m => m.Send(It.IsAny<PlaceBidCommand>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}
