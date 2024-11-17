using MyCarAuction.Api.Features.Users.Common.Models;

namespace MyCarAuction.Api.Features.Users.Queries.GetUser;

public sealed record GetUserResponse : BaseUserResponse
{
    public GetUserResponse(BaseUserResponse original) : base(original)
    {
    }

    public GetUserResponse(Guid id, string name, string email) : base(id, name, email)
    {
    }
}
