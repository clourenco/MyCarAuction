using FluentValidation;

namespace MyCarAuction.Api.Features.Users.Commands.CreateUser;

internal sealed class CreateUserValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .WithMessage("Invalid email address.");
    }
}
