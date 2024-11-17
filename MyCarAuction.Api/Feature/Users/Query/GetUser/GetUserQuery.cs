using MediatR;

namespace MyCarAuction.Api.Features.Users.Queries.GetUser;

internal sealed record GetUserQuery(Guid Id) : IRequest<GetUserResponse>;
