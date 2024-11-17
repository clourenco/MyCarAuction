using MyCarAuction.Api.Features.Users.Common.Model;

namespace MyCarAuction.Api.Features.Users.Queries.GetUser;

internal sealed record GetUserResponse : BaseUserResponse
{
    public GetUserResponse(BaseUserResponse original) : base(original)
    {
    }

    public GetUserResponse(Guid id, string name, string email) : base(id, name, email)
    {
    }
}
