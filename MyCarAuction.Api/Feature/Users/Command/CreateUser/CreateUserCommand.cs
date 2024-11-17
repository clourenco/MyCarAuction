using MediatR;

namespace MyCarAuction.Api.Features.Users.Commands.CreateUser;

internal sealed record CreateUserCommand(string Name, string Email) : IRequest<CreateUserResponse>;
