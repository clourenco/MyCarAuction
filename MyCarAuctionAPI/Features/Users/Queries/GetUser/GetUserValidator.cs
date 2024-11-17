using FluentValidation;

namespace MyCarAuction.Api.Features.Users.Queries.GetUser;

public sealed class GetUserValidator : AbstractValidator<GetUserQuery>
{
    public GetUserValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
