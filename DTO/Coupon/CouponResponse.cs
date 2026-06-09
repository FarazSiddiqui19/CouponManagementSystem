namespace CouponManagementSystem.DTO.Coupon;

public record CouponResponse(
    Guid Id,
    string Code,
    Guid OwnerId,
    DateTime ExpirationDate,
    int Count,
    int ClaimedCount,
    uint RowVersion
);
