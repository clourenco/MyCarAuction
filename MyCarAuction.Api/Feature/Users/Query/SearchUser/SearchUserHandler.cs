using MediatR;
using MyCarAuction.Api.Feature.Users.Query.SearchUser;
using MyCarAuction.Api.Features.Users.Service;

namespace MyCarAuction.Api.Feature.User.Query.SearchUser;

internal sealed class SearchUserHandler : IRequestHandler<SearchUserQuery, IEnumerable<SearchUserResponse>>
{
    private readonly IUserService _userService;

    public SearchUserHandler(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<IEnumerable<SearchUserResponse>> Handle(SearchUserQuery request, CancellationToken cancellationToken)
    {
        var foundUsers = await _userService.SearchUser(request.Name, request.Email, cancellationToken);
        return foundUsers.Select(u => new SearchUserResponse(
            id: u.Id,
            name: u.Name,
            email: u.Email
        ));
    }
}
