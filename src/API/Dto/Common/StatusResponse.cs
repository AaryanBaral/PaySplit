namespace PaySplit.API.Dto.Common
{
    public class StatusResponse
    {
        public Guid Id { get; set; }
        public string Status { get; set; } = default!;
    }
}
