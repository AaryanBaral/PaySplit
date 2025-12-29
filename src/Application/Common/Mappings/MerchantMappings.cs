using PaySplit.Application.Merchants.Command.ActivateMerchant;
using PaySplit.Application.Merchants.Command.CreateMerchant;
using PaySplit.Application.Merchants.Command.DeactivateMerchant;
using PaySplit.Application.Merchants.Command.SuspendMerchant;
using PaySplit.Application.Merchants.Command.UpdateMerchant;
using PaySplit.Application.Merchants.Query.GetAllMerchant;
using PaySplit.Application.Merchants.Query.GetMerchantById;
using PaySplit.Domain.Merchants;

namespace PaySplit.Application.Common.Mappings
{
    public static class MerchantMappings
    {
        public static CreateMerchantResult ToCreateMerchantResult(this Merchant merchant) =>
            new(
                merchantId: merchant.Id,
                tenantId: merchant.TenantId,
                status: merchant.Status.ToString()
            );

        public static UpdateMerchantResult ToUpdateMerchantResult(this Merchant merchant) =>
            new(
                merchantId: merchant.Id,
                name: merchant.Name,
                email: merchant.Email,
                revenueSharePercentage: merchant.RevenueShare.Value,
                status: merchant.Status.ToString()
            );

        public static ActivateMerchantResult ToActivateMerchantResult(this Merchant merchant) =>
            new(merchant.Id, merchant.Status.ToString());

        public static DeactivateMerchantResult ToDeactivateMerchantResult(this Merchant merchant) =>
            new(merchant.Id, merchant.Status.ToString());

        public static SuspendMerchantResult ToSuspendMerchantResult(this Merchant merchant) =>
            new(merchant.Id, merchant.Status.ToString());

        public static GetAllMerchantDto ToGetAllMerchantDto(this Merchant merchant) =>
            new(
                merchant.Id,
                merchant.TenantId,
                merchant.Name,
                merchant.Email,
                merchant.RevenueShare.Value,
                merchant.Status.ToString(),
                merchant.CreatedAtUtc
            );

        public static GetMerchantByIdDto ToGetMerchantByIdDto(this Merchant merchant) =>
            new(
                merchant.Id,
                merchant.TenantId,
                merchant.Name,
                merchant.Email,
                merchant.RevenueShare.Value,
                merchant.Status.ToString(),
                merchant.CreatedAtUtc,
                merchant.DeactivatedAtUtc,
                merchant.SuspendedAtUtc
            );
    }
}
