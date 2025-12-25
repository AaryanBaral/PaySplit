namespace Application.Merchants.Query.GetAllMerchant
{
    public class GetAllMerchantDto
    {
        public Guid Id { get; }
        public Guid TenantId { get; }
        public string Name { get; }
        public string Email { get; }
        public decimal RevenueShare { get; }
        public string Status { get; }
        public DateTimeOffset CreatedAtUtc { get; }

        public GetAllMerchantDto(Guid id, Guid tenantId, string name, string email, decimal revenueShare, string status, DateTimeOffset createdAtUtc)
        {
            Id = id;
            TenantId = tenantId;
            Name = name;
            Email = email;
            RevenueShare = revenueShare;
            Status = status;
            CreatedAtUtc = createdAtUtc;
        }
    }
}
