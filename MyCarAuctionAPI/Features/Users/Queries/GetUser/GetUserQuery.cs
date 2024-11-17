using MediatR;

namespace MyCarAuction.Api.Features.Users.Queries.GetUser;

public sealed record GetUserQuery(Guid Id) : IRequest<GetUserResponse>;
