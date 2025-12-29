using FluentValidation;
using PaySplit.API.Dto.Tenants;

namespace PaySplit.API.Validation;

public sealed class CreateTenantRequestValidator : AbstractValidator<CreateTenantRequest>
{
    public CreateTenantRequestValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.DefaultCurrency)
            .Length(3)
            .When(x => !string.IsNullOrWhiteSpace(x.DefaultCurrency));
    }
}

public sealed class UpdateTenantRequestValidator : AbstractValidator<UpdateTenantRequest>
{
    public UpdateTenantRequestValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
    }
}
