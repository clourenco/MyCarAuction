using Moq;
using MyCarAuction.Api.Features.Users.Service;
using MyCarAuction.Api.Features.Users.Repository;
using MyCarAuction.Api.Domain.Models;
using MyCarAuction.Api.Infrastructure.Data.Entities;
using MyCarAuction.Api.Common.CustomException;
using MyCarAuction.Api.Features.Users.Common.CustomException;

namespace MyCarAuction.Api.Tests.Features.Users.Service;

public class UserServiceTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly UserService _userService;

    public UserServiceTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _userService = new UserService(_userRepositoryMock.Object);
    }

    [Fact]
    public async Task GetUser_ShouldReturnUser_WhenUserExists()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var userEntity = new UserEntity { Id = userId, Name = "John Doe", Email = "john.doe@example.com" };

        _userRepositoryMock
            .Setup(repo => repo.Get(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(userEntity);

        // Act
        var result = await _userService.GetUser(userId, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(userId, result.Id);
        Assert.Equal("John Doe", result.Name);
        Assert.Equal("john.doe@example.com", result.Email);
    }

    [Fact]
    public async Task GetUser_ShouldThrowUserNotFoundException_WhenUserDoesNotExist()
    {
        // Arrange
        var userId = Guid.NewGuid();

        _userRepositoryMock
            .Setup(repo => repo.Get(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((UserEntity)null);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<UserNotFoundException>(() => _userService.GetUser(userId, CancellationToken.None));
        Assert.Equal($"User with id {userId} was not found.", exception.Message);
    }

    [Fact]
    public async Task AddUser_ShouldReturnUser_WhenUserIsSuccessfullyAdded()
    {
        // Arrange
        var user = new User
        (
            id: Guid.NewGuid(),
            name: "Jane Doe",
            email: "jane.doe@example.com"
        );
        var userEntity = new UserEntity { Id = user.Id, Name = user.Name, Email = user.Email };

        _userRepositoryMock
            .Setup(repo => repo.Get(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((UserEntity)null);

        _userRepositoryMock
            .Setup(repo => repo.Create(It.IsAny<UserEntity>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(userEntity);

        // Act
        var result = await _userService.AddUser(user, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(user.Id, result.Id);
        Assert.Equal("Jane Doe", result.Name);
        Assert.Equal("jane.doe@example.com", result.Email);
    }

    [Fact]
    public async Task AddUser_ShouldThrowKeyViolationException_WhenUserIdAlreadyExists()
    {
        // Arrange
        var user = new User
        (
            id: Guid.NewGuid(),
            name: "Jane Doe",
            email: "jane.doe@example.com"
        );

        var userEntity = new UserEntity { Id = user.Id, Name = user.Name, Email = user.Email };

        _userRepositoryMock
            .Setup(repo => repo.Get(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(userEntity);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<KeyViolationException>(() => _userService.AddUser(user, CancellationToken.None));
        Assert.Equal($"The provided id ({user.Id}) for the user is already in use.", exception.Message);
    }

    [Fact]
    public async Task SearchUser_ShouldReturnUsers_WhenUsersExist()
    {
        // Arrange
        var name = "John";
        var email = "example";
        var userEntities = new List<UserEntity>
        {
            new() { Id = Guid.NewGuid(), Name = "John Doe", Email = "john.doe@example.com" },
            new() { Id = Guid.NewGuid(), Name = "John Smith", Email = "john.smith@example.com" }
        };

        _userRepositoryMock
            .Setup(repo => repo.FindUser(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(userEntities);

        // Act
        var result = await _userService.SearchUser(name, email, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        Assert.Contains(result, u => u.Name == "John Doe");
        Assert.Contains(result, u => u.Name == "John Smith");
    }
}
