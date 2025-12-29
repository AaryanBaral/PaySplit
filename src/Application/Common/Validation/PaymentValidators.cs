using FluentValidation;
using PaySplit.Application.Payments.Command.ConfirmPaymentSucceeded;
using PaySplit.Application.Payments.Command.CreateIncomingPayment;
using PaySplit.Application.Payments.Command.CreatePayment;

namespace PaySplit.Application.Common.Validation;

public sealed class CreatePaymentCommandValidator : AbstractValidator<CreatePaymentCommand>
{
    public CreatePaymentCommandValidator()
    {
        RuleFor(x => x.TenantId).NotEmpty();
        RuleFor(x => x.MerchantId).NotEmpty();
        RuleFor(x => x.PaymentAmount).GreaterThan(0);
        RuleFor(x => x.Currency).NotEmpty().Length(3);
    }
}

public sealed class CreateIncomingPaymentCommandValidator : AbstractValidator<CreateIncomingPaymentCommand>
{
    public CreateIncomingPaymentCommandValidator()
    {
        RuleFor(x => x.TenantId).NotEmpty();
        RuleFor(x => x.MerchantId).NotEmpty();
        RuleFor(x => x.PaymentAmount).GreaterThan(0);
        RuleFor(x => x.Currency).NotEmpty().Length(3);
        RuleFor(x => x.ExternalPaymentId).NotEmpty();
    }
}

public sealed class ConfirmPaymentSucceededCommandValidator : AbstractValidator<ConfirmPaymentSucceededCommand>
{
    public ConfirmPaymentSucceededCommandValidator()
    {
        RuleFor(x => x.PaymentId).NotEmpty();
        RuleFor(x => x.CompletedAtUtc).NotEqual(default(DateTimeOffset));
    }
}
