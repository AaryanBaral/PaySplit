using FluentValidation;
using PaySplit.Application.Merchants.Command.ActivateMerchant;
using PaySplit.Application.Merchants.Command.CreateMerchant;
using PaySplit.Application.Merchants.Command.DeactivateMerchant;
using PaySplit.Application.Merchants.Command.SuspendMerchant;
using PaySplit.Application.Merchants.Command.UpdateMerchant;

namespace PaySplit.Application.Common.Validation;

public sealed class CreateMerchantCommandValidator : AbstractValidator<CreateMerchantCommand>
{
    public CreateMerchantCommandValidator()
    {
        RuleFor(x => x.TenantId).NotEmpty();
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.RevenueSharePercentage).GreaterThan(0).LessThan(100);
    }
}

public sealed class UpdateMerchantCommandValidator : AbstractValidator<UpdateMerchantCommand>
{
    public UpdateMerchantCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.RevenueSharePercentage).GreaterThan(0).LessThan(100);
    }
}

public sealed class ActivateMerchantCommandValidator : AbstractValidator<ActivateMerchantCommand>
{
    public ActivateMerchantCommandValidator()
    {
        RuleFor(x => x.MerchantId).NotEmpty();
    }
}

public sealed class DeactivateMerchantCommandValidator : AbstractValidator<DeactivateMerchantCommand>
{
    public DeactivateMerchantCommandValidator()
    {
        RuleFor(x => x.MerchantId).NotEmpty();
    }
}

public sealed class SuspendMerchantCommandValidator : AbstractValidator<SuspendMerchantCommand>
{
    public SuspendMerchantCommandValidator()
    {
        RuleFor(x => x.MerchantId).NotEmpty();
    }
}
