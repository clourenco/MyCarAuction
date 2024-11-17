using FluentValidation;

namespace MyCarAuction.Api.Features.Users.Queries.GetUser;

internal sealed class GetUserValidator : AbstractValidator<GetUserQuery>
{
    public GetUserValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
