using MediatR;

namespace MyCarAuction.Api.Features.Users.Commands.CreateUser;

public sealed record CreateUserCommand(string Name, string Email) : IRequest<CreateUserResponse>;
