using MediatR;
using MyCarAuction.Api.Features.Users.Service;

namespace MyCarAuction.Api.Features.Users.Queries.GetUser;

public sealed class GetUserHandler : IRequestHandler<GetUserQuery, GetUserResponse>
{
    private readonly IUserService _userService;

    public GetUserHandler(IUserService userService)
    {
        _userService = userService ?? throw new ArgumentNullException(nameof(userService));
    }

    public async Task<GetUserResponse> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        var user = await _userService.GetUser(request.Id, cancellationToken);

        return new GetUserResponse(
            id: request.Id,
            name: user.Name,
            email: user.Email
        );
    }
}
