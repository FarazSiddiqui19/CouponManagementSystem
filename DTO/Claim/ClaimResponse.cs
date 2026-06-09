namespace CouponManagementSystem.DTO.Claim;

public record ClaimResponse(
    Guid Id,
    Guid CouponId,
    string CouponCode,
    Guid UserId,
    DateTime ClaimedAt
);
