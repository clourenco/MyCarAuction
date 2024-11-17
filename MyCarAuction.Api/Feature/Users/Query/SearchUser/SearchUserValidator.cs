using FluentValidation;

namespace MyCarAuction.Api.Feature.Users.Query.SearchUser
{
    public class SearchUserValidator : AbstractValidator<SearchUserQuery>
    {
        public SearchUserValidator()
        {
            RuleFor(x => x)
                .Must(HaveAtLeastOneNonEmptyField)
                .WithMessage("At least one field (Name, Email) must be provided.");
        }

        private bool HaveAtLeastOneNonEmptyField(SearchUserQuery query)
        {
            return !string.IsNullOrEmpty(query.Name) || !string.IsNullOrEmpty(query.Email);
        }
    }
}
