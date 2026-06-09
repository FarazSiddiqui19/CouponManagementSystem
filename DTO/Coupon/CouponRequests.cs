namespace CouponManagementSystem.DTO.Coupon;

public record CreateCouponRequest(
    string Code,
    DateTime ExpirationDate,
    int Count
);

public record UpdateCouponRequest(
    DateTime? ExpirationDate,
    int? Count,
    uint RowVersion
);
