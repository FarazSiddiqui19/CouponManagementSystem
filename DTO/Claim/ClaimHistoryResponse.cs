namespace CouponManagementSystem.DTO.Claim;

public class ClaimHistoryResponse
{
    public Guid id { get; set; }
    public Guid couponId { get; set; }
    public string couponCode { get; set; } = string.Empty;
    public DateTime claimedAt { get; set; }
}
