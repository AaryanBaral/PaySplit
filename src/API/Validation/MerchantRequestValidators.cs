using FluentValidation;
using PaySplit.API.Dto.Merchants;

namespace PaySplit.API.Validation;

public sealed class CreateMerchantRequestValidator : AbstractValidator<CreateMerchantRequest>
{
    public CreateMerchantRequestValidator()
    {
        RuleFor(x => x.TenantId).NotEmpty();
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.RevenueSharePercentage).GreaterThan(0).LessThan(100);
    }
}

public sealed class UpdateMerchantRequestValidator : AbstractValidator<UpdateMerchantRequest>
{
    public UpdateMerchantRequestValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.RevenueSharePercentage).GreaterThan(0).LessThan(100);
    }
}
