using FluentValidation;
using PaySplit.Application.Payouts.Commands.ApprovePayout;
using PaySplit.Application.Payouts.Commands.CompletePayout;
using PaySplit.Application.Payouts.Commands.GeneratePayoutRequest;
using PaySplit.Application.Payouts.Commands.RejectPayout;

namespace PaySplit.Application.Common.Validation;

public sealed class GeneratePayoutRequestCommandValidator : AbstractValidator<GeneratePayoutRequestCommand>
{
    public GeneratePayoutRequestCommandValidator()
    {
        RuleFor(x => x.TenantId).NotEmpty();
        RuleFor(x => x.MerchantId).NotEmpty();
        RuleFor(x => x.RequestedByUserId).NotEmpty();
        RuleFor(x => x.RequestedAmount).GreaterThan(0);
        RuleFor(x => x.Currency).NotEmpty().Length(3);
    }
}

public sealed class ApprovePayoutCommandValidator : AbstractValidator<ApprovePayoutCommand>
{
    public ApprovePayoutCommandValidator()
    {
        RuleFor(x => x.TenantId).NotEmpty();
        RuleFor(x => x.PayoutId).NotEmpty();
        RuleFor(x => x.ApprovedByUserId).NotEmpty();
    }
}

public sealed class RejectPayoutCommandValidator : AbstractValidator<RejectPayoutCommand>
{
    public RejectPayoutCommandValidator()
    {
        RuleFor(x => x.TenantId).NotEmpty();
        RuleFor(x => x.PayoutId).NotEmpty();
        RuleFor(x => x.RejectedByUserId).NotEmpty();
        RuleFor(x => x.RejectedAtUtc).NotEqual(default(DateTimeOffset));
    }
}

public sealed class CompletePayoutCommandValidator : AbstractValidator<CompletePayoutCommand>
{
    public CompletePayoutCommandValidator()
    {
        RuleFor(x => x.TenantId).NotEmpty();
        RuleFor(x => x.PayoutId).NotEmpty();
        RuleFor(x => x.CompletedByUserId).NotEmpty();
        RuleFor(x => x.CompletedAtUtc).NotEqual(default(DateTimeOffset));
    }
}
