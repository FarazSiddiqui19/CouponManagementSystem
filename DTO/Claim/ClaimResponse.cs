namespace CouponManagementSystem.DTO.Claim;

public class ClaimResponse
{
    public Guid id { get; set; }
    public Guid couponId { get; set; }
    public string couponCode { get; set; } = string.Empty;
    public Guid userId { get; set; }
    public DateTime claimedAt { get; set; }
}
