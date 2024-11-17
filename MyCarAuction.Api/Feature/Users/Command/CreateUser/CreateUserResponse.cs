using MyCarAuction.Api.Features.Users.Common.Model;

namespace MyCarAuction.Api.Features.Users.Commands.CreateUser;

internal sealed record CreateUserResponse : BaseUserResponse
{
    public CreateUserResponse(BaseUserResponse original) : base(original)
    {
    }

    public CreateUserResponse(Guid id, string name, string email) : base(id, name, email)
    {
    }
}
