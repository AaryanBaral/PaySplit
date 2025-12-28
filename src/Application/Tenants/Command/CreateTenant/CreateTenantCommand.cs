using PaySplit.Application.Common.Abstractions;
using PaySplit.Application.Common.Results;

namespace PaySplit.Application.Tenants.Command.CreateTenant
{
    public class CreateTenantCommand : ICommand<Result<CreateTenantResult>>
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
