namespace CouponManagementSystem.DTO.Auth;

public record AuthResponse(
    string Token,
    Guid UserId,
    string Name,
    string Email
);
