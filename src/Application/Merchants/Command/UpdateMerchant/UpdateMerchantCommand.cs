using PaySplit.Application.Common.Results;
using MediatR;

namespace PaySplit.Application.Merchants.Command.UpdateMerchant
{
    public class UpdateMerchantCommand : IRequest<Result<UpdateMerchantResult>>
    {
        public Guid Id { get; }
        public string Name { get; }
        public string Email { get; }
        public decimal RevenueSharePercentage { get; }

        public UpdateMerchantCommand(Guid id, string name, string email, decimal revenueSharePercentage)
        {
            Id = id;
            Name = name;
            Email = email;
            RevenueSharePercentage = revenueSharePercentage;
        }
    }
}
