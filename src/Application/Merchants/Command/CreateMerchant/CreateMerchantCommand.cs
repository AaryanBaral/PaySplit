using PaySplit.Application.Common.Abstractions;
using PaySplit.Application.Common.Results;

namespace PaySplit.Application.Merchants.Command.CreateMerchant
{
    public class CreateMerchantCommand : ICommand<Result<CreateMerchantResult>>
    {
        public Guid TenantId { get; }
        public string Name { get; }
        public string Email { get; }
        public decimal RevenueSharePercentage { get; }

        public CreateMerchantCommand(Guid tenantId, string name, string email, decimal revenueSharePercentage)
        {
            TenantId = tenantId;
            Name = name;
            Email = email;
            RevenueSharePercentage = revenueSharePercentage;
        }
    }
}
