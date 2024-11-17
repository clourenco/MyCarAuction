using MyCarAuction.Api.Features.Users.Common.Model;

namespace MyCarAuction.Api.Feature.User.Query.SearchUser;

internal sealed record SearchUserResponse : BaseUserResponse
{
    public SearchUserResponse(BaseUserResponse original) : base(original)
    {
    }

    public SearchUserResponse(Guid id, string name, string email) : base(id, name, email)
    {
    }
}
