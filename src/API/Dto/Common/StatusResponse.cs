namespace PaySplit.API.Dto.Common
{
    public sealed record StatusResponse
    {
        public Guid Id { get; init; }
        public required string Status { get; init; }
    }
}
