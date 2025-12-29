using FluentValidation;
using PaySplit.Application.Tenants.Command.ActivateTenant;
using PaySplit.Application.Tenants.Command.CreateTenant;
using PaySplit.Application.Tenants.Command.DeactivateTenant;
using PaySplit.Application.Tenants.Command.SuspendTenant;
using PaySplit.Application.Tenants.Command.UpdateTenant;

namespace PaySplit.Application.Common.Validation;

public sealed class CreateTenantCommandValidator : AbstractValidator<CreateTenantCommand>
{
    public CreateTenantCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.DefaultCurrency)
            .Length(3)
            .When(x => !string.IsNullOrWhiteSpace(x.DefaultCurrency));
    }
}

public sealed class RenameTenantCommandValidator : AbstractValidator<RenameTenantCommand>
{
    public RenameTenantCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Name).NotEmpty();
    }
}

public sealed class ActivateTenantCommandValidator : AbstractValidator<ActivateTenantCommand>
{
    public ActivateTenantCommandValidator()
    {
        RuleFor(x => x.TenantId).NotEmpty();
    }
}

public sealed class DeactivateTenantCommandValidator : AbstractValidator<DeactivateTenantCommand>
{
    public DeactivateTenantCommandValidator()
    {
        RuleFor(x => x.TenantId).NotEmpty();
    }
}

public sealed class SuspendTenantCommandValidator : AbstractValidator<SuspendTenantCommand>
{
    public SuspendTenantCommandValidator()
    {
        RuleFor(x => x.TenantId).NotEmpty();
    }
}
