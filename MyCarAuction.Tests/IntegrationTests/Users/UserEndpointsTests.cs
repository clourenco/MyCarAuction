using FluentAssertions;
using MediatR;
using Moq;
using MyCarAuction.Api.Features.Users.Commands.CreateUser;
using MyCarAuction.Tests.Factories;
using System.Net.Http.Json;
using System.Net;
using MyCarAuction.Api.Features.Users.Queries.GetUser;
using MyCarAuction.Api.Feature.User.Query.SearchUser;
using MyCarAuction.Api.Feature.Users.Query.SearchUser;

namespace MyCarAuction.Tests.IntegrationTests.Users;

public class UserEndpointsTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly Mock<IMediator> _mediatorMock;

    public UserEndpointsTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
        _mediatorMock = factory.MediatorMock;
    }

    [Fact]
    public async Task AddUser_ShouldReturnCreated()
    {
        // Arrange
        var command = new CreateUserCommand(
            Name: "John Doe",
            Email: "johndoe@example.com"
        );

        var expectedResult = new CreateUserResponse(
            id: Guid.NewGuid(),
            name: "John Doe",
            email: "johndoe@example.com"
        );

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<CreateUserCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResult);

        // Act
        var response = await _client.PostAsJsonAsync("/api/users", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var location = response.Headers.Location;
        location.Should().NotBeNull();

        _mediatorMock.Verify(m => m.Send(It.IsAny<CreateUserCommand>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetUser_ShouldReturnOk()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var expectedResult = new GetUserResponse(
            id: userId,
            name: "John Doe",
            email: "johndoe@example.com"
        );

        _mediatorMock
            .Setup(m => m.Send(It.Is<GetUserQuery>(q => q.Id == userId), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResult);

        // Act
        var response = await _client.GetAsync($"/api/users/{userId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        _mediatorMock.Verify(m => m.Send(It.Is<GetUserQuery>(q => q.Id == userId), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task SearchUsers_ShouldReturnOk()
    {
        // Arrange
        var queryResults = new List<SearchUserResponse>()
        {
            new(
                id: Guid.NewGuid(),
                name: "John Doe",
                email: "johndoe@example.com"
            )
        };

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<SearchUserQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(queryResults);

        // Act
        var response = await _client.GetAsync("/api/users?name=John&email=johndoe@example.com");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        _mediatorMock.Verify(m => m.Send(It.IsAny<SearchUserQuery>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}
