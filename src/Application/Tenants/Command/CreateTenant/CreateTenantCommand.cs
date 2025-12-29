using PaySplit.Application.Common.Results;
using MediatR;

namespace PaySplit.Application.Tenants.Command.CreateTenant
{
    public class CreateTenantCommand : IRequest<Result<CreateTenantResult>>
    {
        public string Name { get; } = default!;
        public string? DefaultCurrency { get; }

        public CreateTenantCommand(string name, string? defaultCurrency = null)
        {
            Name = name;
            DefaultCurrency = defaultCurrency;
        }
    }
}
