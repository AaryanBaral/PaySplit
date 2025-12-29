

using PaySplit.Domain.Common;

namespace PaySplit.Application.Common.Models
{
    public sealed record MerchantBalanceDto(
        Money Available,
        Money Posted,
        Money Pending);
}
