using MyCarAuction.Api.Features.Users.Common.Models;

namespace MyCarAuction.Api.Features.Users.Commands.CreateUser;

public sealed record CreateUserResponse : BaseUserResponse
{
    public CreateUserResponse(BaseUserResponse original) : base(original)
    {
    }

    public CreateUserResponse(Guid id, string name, string email) : base(id, name, email)
    {
    }
}
