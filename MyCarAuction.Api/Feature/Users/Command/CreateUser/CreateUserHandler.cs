using MediatR;
using MyCarAuction.Api.Domain.Models;
using MyCarAuction.Api.Features.Users.Service;

namespace MyCarAuction.Api.Features.Users.Commands.CreateUser;

internal sealed class CreateUserHandler : IRequestHandler<CreateUserCommand, CreateUserResponse>
{
    private readonly IUserService _userService;

    public CreateUserHandler(IUserService userService)
    {
        _userService = userService ?? throw new ArgumentNullException(nameof(userService));
    }

    public async Task<CreateUserResponse> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        User user = new(
            id: Guid.NewGuid(),
            name: request.Name,
            email: request.Email
        );

        var addedUser = await _userService.AddUser(user, cancellationToken);

        return new CreateUserResponse(
            id: addedUser.Id,
            name: addedUser.Name,
            email: addedUser.Email
        );
    }
}
