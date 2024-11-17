using MyCarAuction.Api.Common.CustomException;
using MyCarAuction.Api.Domain.Models;
using MyCarAuction.Api.Features.Users.Common.CustomException;
using MyCarAuction.Api.Features.Users.Repository;
using MyCarAuction.Api.Infrastructure.Data.Entities;

namespace MyCarAuction.Api.Features.Users.Service;

internal sealed class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
    }

    public async Task<User> GetUser(Guid id, CancellationToken cancellationToken)
    {
        var existentUser = await _userRepository.Get(id, cancellationToken);

        if (existentUser != null)
            return MapToModel(existentUser);
        else
            throw new UserNotFoundException($"User with id {id} was not found.");
    }

    public async Task<User> AddUser(User user, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(user);
        var existentUser = await _userRepository.Get(user.Id, cancellationToken);

        if (existentUser != null)
            throw new KeyViolationException($"The provided id ({user.Id}) for the user is already in use.");

        UserEntity userEntity = MapToEntity(user);

        var addedUser = await _userRepository.Create(userEntity, cancellationToken);

        return MapToModel(addedUser);
    }

    public async Task<IEnumerable<User>> SearchUser(string name, string email, CancellationToken cancellationToken)
    {
        var userEntities = await _userRepository.FindUser(name, email, cancellationToken);
        return userEntities.Select(e => MapToModel(e));
    }

    #region Private methods

    private static User MapToModel(UserEntity entity)
        => new(
            id: entity.Id,
            name: entity.Name,
            email: entity.Email
        );

    private UserEntity MapToEntity(User model)
        => new()
        {
            Id = model.Id,
            Name = model.Name,
            Email = model.Email
        };

    #endregion
}
