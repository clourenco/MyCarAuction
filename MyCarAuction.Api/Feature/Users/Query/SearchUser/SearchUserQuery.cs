using MediatR;
using MyCarAuction.Api.Feature.User.Query.SearchUser;

namespace MyCarAuction.Api.Feature.Users.Query.SearchUser;

public sealed record SearchUserQuery(string? Name, string? Email) : IRequest<IEnumerable<SearchUserResponse>>;
